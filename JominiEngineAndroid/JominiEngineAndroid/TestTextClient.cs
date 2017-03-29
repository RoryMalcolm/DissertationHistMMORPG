using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Lidgren.Network;
using System.Security.Cryptography;
using System.Net;
using System.IO;
using ProtoBuf;
using System.Security.Cryptography.X509Certificates;
/// <summary>
/// Extends the ConcurrentQueue to fire an event whenever a new item is enqueued
/// </summary>
/// <typeparam name="T"></typeparam>
public class ConcurrentQueueWithEvent<T> : ConcurrentQueue<T>
{
	public EventWaitHandle eventWaiter { get; set; }

	public ConcurrentQueueWithEvent() : base()
	{
		eventWaiter = new EventWaitHandle(false, EventResetMode.AutoReset);
	}

	/// <summary>
	/// Add a new item to the queue and set the EventWaitHandle
	/// </summary>
	/// <param name="t">Item to be enqueued</param>
	public new void Enqueue(T t)
	{
		base.Enqueue(t);
		eventWaiter.Set();
	}


}
public partial class TextTestClient
{
	public ConcurrentQueueWithEvent<ProtoMessage.ProtoMessage> protobufMessageQueue;
	public ConcurrentQueueWithEvent<string> stringMessageQueue;
	public Network net;
	public string playerID;


	public TextTestClient()
	{
		protobufMessageQueue = new ConcurrentQueueWithEvent<ProtoMessage.ProtoMessage>();
		stringMessageQueue = new ConcurrentQueueWithEvent<string>();
	}
	/*************************************
	 * General Commands ***
	 * **********************************/
	/// <summary>
	/// Log in to the server
	/// </summary>
	/// <param name="user">Username</param>
	/// <param name="pass">Password</param>
	public void LogInAndConnect(string user, string pass, byte[] key = null)
	{

		net = new Network(this, key);
		net.Connect(user, pass);
		this.playerID = user;
	}

	public bool IsConnectedAndLoggedIn()
	{
		if (net.GetConnectionStatusString().Equals("Connected") && net.loggedIn)
		{
			return true;
		}
		return false;
	}

	/// <summary>
	/// Clear both message queues- used in tests where we don't care about the login messages
	/// </summary>
	public void ClearMessageQueues()
	{
		// Note- With ConcurrentQueues, it is preferrable to discard the previous queue than to attempt to Dequeue all elements in case the queue is currently being written to
		protobufMessageQueue = new ConcurrentQueueWithEvent<ProtoMessage.ProtoMessage>();

		stringMessageQueue = new ConcurrentQueueWithEvent<string>();


	}
	public void LogOut()
	{
		net.Disconnect();
	}

	/// <summary>
	/// Gets the next message from the server by repeatedly polling the message queue. 
	/// </summary>
	/// <returns>Message from server</returns>
	/// <throws>TaskCanceledException if task is cancelled</throws>
	private ProtoMessage.ProtoMessage CheckForProtobufMessage()
	{
		ProtoMessage.ProtoMessage m = null;
		var waitHandles = new WaitHandle[] { protobufMessageQueue.eventWaiter, net.ctSource.Token.WaitHandle };
		while (!protobufMessageQueue.TryDequeue(out m))
		{
			EventWaitHandle.WaitAny(waitHandles);
			net.ctSource.Token.ThrowIfCancellationRequested();
		}
		return m;
	}

	/// <summary>
	/// Gets the next message from the server by repeatedly polling the message queue
	/// </summary>
	/// <returns>Message from server</returns>
	/// <throws>TaskCanceledException if task is cancelled</throws>
	private string CheckForStringMessage()
	{
		string s = null;
		var waitHandles = new WaitHandle[] { stringMessageQueue.eventWaiter, net.ctSource.Token.WaitHandle };
		while (!stringMessageQueue.TryDequeue(out s))
		{
			EventWaitHandle.WaitAny(waitHandles);
			net.ctSource.Token.ThrowIfCancellationRequested();
		}
		return s;
	}

	/// <summary>
	/// Gets the next message recieved from the server
	/// </summary>
	/// <returns>Task containing the reply as a result</returns>
	public async Task<ProtoMessage.ProtoMessage> GetReply(string id = null)
	{

		ProtoMessage.ProtoMessage reply = await (Task.Run(() => CheckForProtobufMessage()));
#if DEBUG
		if (reply == null)
		{
		}
		else
		{
		}
#endif
		return reply;
	}

	/// <summary>
	/// Gets the next message recieved from the server
	/// </summary>
	/// <returns>Task containing the reply as a result</returns>
	public async Task<string> GetServerMessage()
	{

		string reply = await (Task.Run(() => CheckForStringMessage()));

		return reply;
	}
	protected virtual void Dispose(bool dispose)
	{
		net.Dispose();
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}
}


public partial class TextTestClient
{

	public class Network : IDisposable
	{
		private TextTestClient tClient;
		RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
		HashAlgorithm hash = new SHA256Managed();
		public NetClient client = null;
		private NetConnection connection;
		private string user;
		private string pass;
		private IPAddress ip = NetUtility.Resolve("localhost");
		private int port = 8000;
		private NetEncryption alg = null;
		/// <summary>
		/// Optional- set encryption key manually for use in testing
		/// </summary>
		private byte[] key;
		public bool autoLogIn { get; set; }
		public bool loggedIn { get; set; }
		public CancellationTokenSource ctSource;

		public Network(TextTestClient tc, byte[] key = null)
		{
			this.tClient = tc;
			autoLogIn = true;
			this.key = key;
			ctSource = new CancellationTokenSource();
			InitializeClient();
		}


		public string GetConnectionStatusString()
		{
			return client.ConnectionStatus.ToString();
		}


		void InitializeClient()
		{
			NetPeerConfiguration config = new NetPeerConfiguration("test");
			config.SetMessageTypeEnabled(NetIncomingMessageType.ConnectionLatencyUpdated, true);
			config.ConnectionTimeout = 3000f;
			client = new NetClient(config);

		}
		public void Connect(string username, string pass)
		{
			user = username;
			this.pass = pass;
			client.Start();
			string host = ip.ToString();
			// remember to encrypt the bloody thing in the final
			if (username != null)
			{
				NetOutgoingMessage msg = client.CreateMessage(username);
				msg.Write("TestString");
				NetConnection c = client.Connect(host, port, msg);
			}
			else
			{
				connection = client.Connect(host, port);
			}
			// Start listening for responses
			Thread t_reader = new Thread(new ThreadStart(this.read));
			t_reader.Start();

		}

		public void Disconnect()
		{
			ctSource.Cancel();
			if (!(client.ConnectionStatus == NetConnectionStatus.None || client.ConnectionStatus == NetConnectionStatus.Disconnected))
			{
				client.Disconnect("Log out");
			}
			Dispose();
			client.Shutdown("Exit");
		}

		/// <summary>
		/// Computes the hash of a salt appended to source byte array
		/// </summary>
		/// <param name="toHash">bytes to be hashed</param>
		/// <param name="salt">salt</param>
		/// <returns>computed hash</returns>
		public byte[] ComputeHash(byte[] toHash, byte[] salt)
		{
			byte[] fullHash = new byte[toHash.Length + salt.Length];
			toHash.CopyTo(fullHash, 0);
			salt.CopyTo(fullHash, toHash.Length);
			byte[] hashcode = hash.ComputeHash(fullHash);
			return hashcode;
		}

		public void Send(ProtoMessage.ProtoMessage message, bool encrypt = true)
		{
			NetOutgoingMessage msg = client.CreateMessage();
			MemoryStream ms = new MemoryStream();
			try
			{
				Serializer.SerializeWithLengthPrefix<ProtoMessage.ProtoMessage>(ms, message, ProtoBuf.PrefixStyle.Fixed32);
				msg.Write(ms.GetBuffer());
				if (alg != null && encrypt)
				{
					msg.Encrypt(alg);
				}
				var result = client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
				client.FlushSendQueue();

			}
			catch (Exception e)
			{
			}

		}

		public void ComputeAndSendHashAndKey(ProtoMessage.ProtoLogIn salts, byte[] key)
		{

			string hashstring = "";
			foreach (byte b in salts.userSalt)
			{
				hashstring += b.ToString();
			}
			string sessSalt = "";
			foreach (byte b in salts.sessionSalt)
			{
				sessSalt += b.ToString();
			}
			byte[] passBytes = Encoding.UTF8.GetBytes(pass);
			byte[] hashPassword = ComputeHash(passBytes, salts.userSalt);
			string passHash = "";
			foreach (byte b in hashPassword)
			{
				passHash += b.ToString();
			}
			byte[] hashFull = ComputeHash(hashPassword, salts.sessionSalt);
			string fullHash = "";
			foreach (byte b in hashFull)
			{
				fullHash += b.ToString();
			}
			ProtoMessage.ProtoLogIn response = new ProtoMessage.ProtoLogIn();
			response.userSalt = hashFull;
			response.ActionType = ProtoMessage.Actions.LogIn;
			response.Key = key;
			Send(response, false);
		}

		/// <summary>
		/// Validates the certificate supplied by the server, and also creates a symmetric encryption key
		/// </summary>
		/// <returns><c>true</c>, if certificate was validated, <c>false</c> otherwise.</returns>
		/// <param name="login">ProtoLogin containing certificate</param>
		public bool ValidateCertificateAndCreateKey(ProtoMessage.ProtoLogIn login, out byte[] key)
		{
			if (login == null || login.certificate == null)
			{
				key = null;
				return false;
			}
			else
			{
				try
				{
					// Get certificate
					X509Certificate2 cert = new X509Certificate2(login.certificate);
					RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)cert.PublicKey.Key;
#if DEBUG
					if (this.key != null)
					{
						if (this.key.Length == 0)
						{
							alg = new NetAESEncryption(client);
						}
						else
						{
							alg = new NetAESEncryption(client,
								this.key, 0, this.key.Length);
						}
						key = rsa.Encrypt(this.key, false);
					}
					else
					{
						// If no key, do not use an encryption algorithm
						alg = null;
						key = null;
					}
#else
						// Create a new symmetric key
						TripleDES des = TripleDESCryptoServiceProvider.Create();
						des.GenerateKey();
						// Encrypt key with server's public key
						this.key = des.Key;
						key = rsa.Encrypt(des.Key, false);
						// Initialise the algoitm
						alg = new NetAESEncryption(client, des.Key, 0, des.Key.Length);
						Console.WriteLine("CLIENT: my unencrypted key:");
						foreach (var bite in des.Key)
						{
						Console.Write(bite.ToString());
						}
#endif
					// Validate certificate
					if (!cert.Verify())
					{
						X509Chain CertificateChain = new X509Chain();
						//If you do not provide revokation information, use the following line.
						CertificateChain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
						bool IsCertificateChainValid = CertificateChain.Build(cert);
						if (!IsCertificateChainValid)
						{
							for (int i = 0; i < CertificateChain.ChainStatus.Length; i++)
							{
							}
							// TODO change to false after testing
							return true;
						}

					}
					// temporary certificate validation fix
					return true;
					//return cert.Verify();
				}
				catch (Exception e)
				{
					Console.Error.WriteLine("A problem occurred when parsing certificate from bytes: \n" + "type: " + e.GetType().FullName + "\n " + ", source: " + e.Source + "\n message: " + e.Message);
					key = null;
					return false;
				}
			}
		}

		public void read()
		{
			while (client.Status == NetPeerStatus.Running && !ctSource.Token.IsCancellationRequested)
			{

				WaitHandle.WaitAny(new WaitHandle[] { client.MessageReceivedEvent, ctSource.Token.WaitHandle });
				NetIncomingMessage im;
				while ((im = client.ReadMessage()) != null && !ctSource.IsCancellationRequested)
				{

					switch (im.MessageType)
					{
						case NetIncomingMessageType.DebugMessage:
						case NetIncomingMessageType.ErrorMessage:
						case NetIncomingMessageType.WarningMessage:
						case NetIncomingMessageType.VerboseDebugMessage:
						case NetIncomingMessageType.Data:
							try
							{
								if (alg != null)
								{
									im.Decrypt(alg);
								}
								MemoryStream ms = new MemoryStream(im.Data);
								ProtoMessage.ProtoMessage m = null;
								try
								{
									m = Serializer.DeserializeWithLengthPrefix<ProtoMessage.ProtoMessage>(ms, PrefixStyle.Fixed32);

								}
								catch (Exception e)
								{
									// Attempt to read string and add to message queue
									string s = im.ReadString();
									if (!string.IsNullOrWhiteSpace(s))
									{

										tClient.stringMessageQueue.Enqueue(s);
									}
								}
								if (m != null)
								{
									if (m.ResponseType == ProtoMessage.DisplayMessages.LogInSuccess)
									{
										loggedIn = true;
										tClient.protobufMessageQueue.Enqueue(m);
									}
									else
									{
										if (m.ActionType == ProtoMessage.Actions.Update)
										{
											// Don't do anything at the moment for updates
										}
										else
										{
											tClient.protobufMessageQueue.Enqueue(m);
											if (m.ActionType == ProtoMessage.Actions.LogIn && m.ResponseType == ProtoMessage.DisplayMessages.None)
											{
												byte[] key = null;
												if (ValidateCertificateAndCreateKey(m as ProtoMessage.ProtoLogIn, out key))
												{
													ComputeAndSendHashAndKey(m as ProtoMessage.ProtoLogIn, key);
												}
											}
											else
											{
												// Attempt to read string and add to message queue
												string s = im.ReadString();
												if (!string.IsNullOrWhiteSpace(s))
												{

													tClient.stringMessageQueue.Enqueue(s);

												}
											}
										}

									}


								}
							}
							catch (Exception e)
							{
							}
							break;
						case NetIncomingMessageType.StatusChanged:

							NetConnectionStatus status = (NetConnectionStatus)im.ReadByte();
							//MemoryStream ms2 = new MemoryStream(im.SenderConnection.RemoteHailMessage.Data);
							if (status == NetConnectionStatus.Connected)
							{

								if (im.SenderConnection.RemoteHailMessage != null)
								{
									try
									{
										MemoryStream ms2 = new MemoryStream(im.SenderConnection.RemoteHailMessage.Data);
										ProtoMessage.ProtoMessage m = Serializer.DeserializeWithLengthPrefix<ProtoMessage.ProtoMessage>(ms2, PrefixStyle.Fixed32);
										if (m != null)
										{
											tClient.protobufMessageQueue.Enqueue(m);
											if (m.ActionType == ProtoMessage.Actions.LogIn && m.ResponseType == ProtoMessage.DisplayMessages.None)
											{
												byte[] key = null;
												if (ValidateCertificateAndCreateKey(m as ProtoMessage.ProtoLogIn, out key))
												{
													if (autoLogIn)
													{
														ComputeAndSendHashAndKey(m as ProtoMessage.ProtoLogIn, key);
													}
												}
												else
												{
													client.Disconnect("Invalid Certificate");
												}

											}

										}
									}
									catch (Exception e)
									{

									}
								}
								break;
							}
							else if (status == NetConnectionStatus.Disconnected)
							{
								string reason = im.ReadString();
								if (!string.IsNullOrEmpty(reason))
								{

									tClient.stringMessageQueue.Enqueue(reason);



								}
							}
							if (im.SenderConnection.RemoteHailMessage != null && (NetConnectionStatus)im.ReadByte() == NetConnectionStatus.Connected)
							{

							}
							break;
						case NetIncomingMessageType.ConnectionLatencyUpdated:
							break;
						default:
							break;
					}
					client.Recycle(im);
				}
			}
#if DEBUG
#endif
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);

		}

		protected virtual void Dispose(bool dispose)
		{
			crypto.Dispose();
			hash.Dispose();
		}
	}
}

