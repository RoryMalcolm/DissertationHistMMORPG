package com.example.user.myapplication;

import android.os.AsyncTask;

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

import javax.crypto.spec.SecretKeySpec;

/**
 * Created by User on 21/03/2017.
 */

public class ClientOperations extends AsyncTask<String, Void, Boolean>{
    private String clientUsername;
    private InetAddress IP;
    private int port = 8000;
    private DatagramSocket socket;
    private String pass;
    private byte[] key;
    MessageDigest md = MessageDigest.getInstance("SHA-256");
    SecretKeySpec secretKey;

    public ClientOperations() throws NoSuchAlgorithmException {
        try {
            IP = InetAddress.getByName("10.0.2.2");
        } catch (UnknownHostException e) {
            e.printStackTrace();
        }
        try {
            socket = new DatagramSocket(port);
        } catch (SocketException e) {
            e.printStackTrace();
        }
    }

    @Override
    protected Boolean doInBackground(String... params) {
        pass = params[0];
        clientUsername = params[1];
        try {

            socket.connect(InetAddress.getByName("10.0.2.2"), 8000);
            try {
                /*byte[] sendPacket = clientUsername.getBytes();
                byte[] testString = "TestString".getBytes();
                */
                byte[] packetCombined = new byte[17];
                packetCombined[0]	=5	;
                packetCombined[1]	=104;
                packetCombined[2]	=101;
                packetCombined[3]	=108;
                packetCombined[4]	=101;
                packetCombined[5]	=110;
                packetCombined[6]	=10	;
                packetCombined[7]	=84	;
                packetCombined[8]	=101;
                packetCombined[9]	=115;
                packetCombined[10]=	116	;
                packetCombined[11]=	83	;
                packetCombined[12]=	116	;
                packetCombined[13]=	114	;
                packetCombined[14]=	105	;
                packetCombined[15]=	110	;
                packetCombined[16]=	103	;

                /*byte[] packetCombined = new byte[sendPacket.length + testString.length];
                int counter = 0;
                while(counter < sendPacket.length){
                    packetCombined[counter] = sendPacket[counter];
                    counter++;
                }
                int secondCounter = 0;
                while (secondCounter < testString.length){
                    packetCombined[counter] = testString[secondCounter];
                    counter++;
                    secondCounter++;
                }*/
                DatagramPacket loginPacket = new DatagramPacket(packetCombined, packetCombined.length);
                socket.send(loginPacket);
            } catch (IOException e) {
                e.printStackTrace();
            }
            HistMmorpg.ProtoLogIn.Builder protoLogin = HistMmorpg.ProtoLogIn.newBuilder();
            HistMmorpg.ProtoLogIn logInSalts = protoLogin.build();
        } catch (UnknownHostException e) {
            e.printStackTrace();
        }
        return true;
    }

    public byte[] LidgrenSend(byte[] packetForModify){
        byte[] sendable = new byte[packetForModify.length+2];
        sendable[0] = 5;
        int counter = 1;
        for (byte b:
             packetForModify) {
            sendable[counter] = packetForModify[counter-1];
            counter++;
        }
        sendable[counter] = 10;
        return sendable;
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
        byte[] passBytes;
        try {
            passBytes = pass.getBytes("UTF8");
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
        if(key != null) {
            response.setKey(ByteString.copyFrom(key));
        }
        HistMmorpg.ProtoLogIn sendPacket = response.build();
        Send(sendPacket.toByteArray());
        } catch (UnsupportedEncodingException e) {
            e.printStackTrace();
        }
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
        /*if (login == null || login.getCertificate() == null)
        {
            key = null;
            return false;
        }
        else {
            try {
                // Get certificate
                X509Certificate cert = new X509Certificate(login.getCertificate()) {
                };
                secretKey = new SecretKeySpec(key, "AES");
                if (this.key != null) {
                    if (this.key.length == 0) {
                        alg = new NetAESEncryption(client);
                    } else {
                        alg = new NetAESEncryption(client,
                                this.key, 0, this.key.Length);
                    }
                    key = secretKey.getEncoded();
                } else {
                    // If no key, do not use an encryption algorithm
                    alg = null;
                    key = null;
                }
                // Validate certificate
                if (!cert.Verify()) {
                    X509Chain CertificateChain = new X509Chain();
                    //If you do not provide revokation information, use the following line.
                    CertificateChain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
                    boolean IsCertificateChainValid = CertificateChain.Build(cert);
                    if (!IsCertificateChainValid) {
                        for (int i = 0; i < CertificateChain.ChainStatus.Length; i++) {
                        }
                        // TODO change to false after testing
                        return true;
                    }

                }
                // temporary certificate validation fix
                return true;
                //return cert.Verify();
            } catch (Exception e) {
                key = null;
                return false;
            }
        }*/
        return true;
    }

}
