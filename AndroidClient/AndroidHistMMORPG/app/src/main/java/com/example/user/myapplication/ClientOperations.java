package com.example.user.myapplication;

import java.io.IOException;
import java.net.*;
import java.io.FileInputStream;
import java.io.InputStream;
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

    public ClientOperations(){
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
    public boolean Connect(String username, String password) {
        clientUsername = username;
        try {
            socket.connect(InetAddress.getByName("10.0.2.2"), 8000);
        } catch (UnknownHostException e) {
            e.printStackTrace();
        }
        return true;
    }

    public void Send(HistMmorpg.ProtoMessage message){
        DatagramPacket outgoingPacket = new DatagramPacket(message.toByteArray(), message.toByteArray().length);
        try {
            socket.send(outgoingPacket);
        } catch (IOException e) {
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
}
