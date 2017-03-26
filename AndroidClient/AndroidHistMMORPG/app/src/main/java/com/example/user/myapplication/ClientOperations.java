package com.example.user.myapplication;

import com.google.protobuf.ByteString;

import java.io.IOException;
import java.io.UnsupportedEncodingException;
import java.net.*;
import java.io.FileInputStream;
import java.io.InputStream;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.security.cert.Certificate;
import java.security.cert.CertificateException;
import java.security.cert.CertificateFactory;
import java.security.cert.X509Certificate;
import java.security.interfaces.RSAKey;

/**
 * Created by User on 21/03/2017.
 */

public class ClientOperations {
    private String clientUsername;
    private InetAddress IP;
    private int port = 8000;
    private DatagramSocket socket;
    private String pass;
    private byte[] key;
    MessageDigest md = MessageDigest.getInstance("SHA-256");

    public ClientOperations() throws NoSuchAlgorithmException {
        try {
            IP = InetAddress.getByName("10.0.2.2");
        } catch (UnknownHostException e) {
            e.printStackTrace();
        }
        try {
            socket = new DatagramSocket(port, IP);
        } catch (SocketException e) {
            e.printStackTrace();
        }
    }

    public byte[] ComputeHash(byte[] toHash, byte[] salt)
    {
        byte[] fullHash = new byte[toHash.length + salt.length];
        int counter = 0;
        while(counter<salt.length){
            fullHash[counter] = salt[counter];
        }
        int newHashCounter= 0;
        while(newHashCounter< toHash.length){
            fullHash[counter] = toHash[newHashCounter];
            newHashCounter++;
            counter ++;
        }
        byte[] hashcode = md.digest(fullHash);
        return hashcode;
    }


    public boolean Connect(String username, String password) {
        pass = password;
        clientUsername = username;
        try {
            socket.connect(InetAddress.getByName("10.0.2.2"), 8000);
            HistMmorpg.ProtoLogIn.Builder protoLogin = HistMmorpg.ProtoLogIn.newBuilder();
            HistMmorpg.ProtoLogIn logInSalts = protoLogin.build();
            ComputeAndSendHashAndKey(logInSalts, key);

        } catch (UnknownHostException e) {
            e.printStackTrace();
        }
        return true;
    }

    public void Send(byte[] message){
        DatagramPacket outgoingPacket = new DatagramPacket(message, message.length);
        try {
            socket.send(outgoingPacket);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    public void ComputeAndSendHashAndKey(HistMmorpg.ProtoLogIn salts, byte[] key)
    {
        String hashstring = "";
        for (byte b : salts.getUserSalt()
             ) {
            hashstring += b;
        }
        String sessSalt = "";
        for (byte b: salts.getSessionSalt()
             ) {
            sessSalt += b;

        }
        byte[] passBytes = new byte[0];
        try {
            passBytes = pass.getBytes("UTF8");
        } catch (UnsupportedEncodingException e) {
            e.printStackTrace();
        }
        byte[] hashPassword = ComputeHash(passBytes, salts.getUserSalt().toByteArray());
        String passHash = "";
        for (byte b:
             hashPassword) {
            passHash += b;

        }
        byte[] hashFull = ComputeHash(hashPassword, salts.getSessionSalt().toByteArray());
        String fullHash = "";
        for (byte b:
             hashFull) {
            fullHash += b;

        }
        HistMmorpg.ProtoLogIn.Builder response = HistMmorpg.ProtoLogIn.newBuilder();
        response.setUserSalt(ByteString.copyFrom(hashFull));
        response.setKey(ByteString.copyFrom(key));
        Send(response.build().toByteArray());
    }


    public HistMmorpg.ProtoMessage Receive(){
        HistMmorpg.ProtoMessage.Builder message = HistMmorpg.ProtoMessage.newBuilder();
        HistMmorpg.ProtoMessage msg = message.build();
        byte [] buf = new byte[msg.getSerializedSize()];
        DatagramPacket datagramPacket = new DatagramPacket(buf, msg.getSerializedSize());
        try {
            socket.receive(datagramPacket);
        } catch (IOException e) {
            e.printStackTrace();
        }
        datagramPacket.getData();
        return msg;
    }

    public boolean ValidateCertificateAndCreateKey(HistMmorpg.ProtoLogIn login, byte[] key)
    {
        if (login == null || login.getCertificate() == null)
        {
            key = null;
            return false;
        }
        else
        {
            try
            {
                // Get certificate
                X509Certificate2 cert = new X509Certificate2(login.getCertificate());
                RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)cert.PublicKey.Key;
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
                // Validate certificate
                if (!cert.Verify())
                {
                    X509Chain CertificateChain = new X509Chain();
                    //If you do not provide revokation information, use the following line.
                    CertificateChain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
                    boolean IsCertificateChainValid = CertificateChain.Build(cert);
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
                key = null;
                return false;
            }
        }
    }

}
