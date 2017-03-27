package com.example.user.myapplication;

import java.security.NoSuchAlgorithmException;

/**
 * Created by User on 19/03/2017.
 */

public class PlayerOperations {

    ClientOperations clientOperations;
    public enum PlayerDirections{
        NE, NW, W, E, SE, SW
    }

    public PlayerOperations() {
        try {
             new ClientOperations().execute("helen", "potato");
        } catch (NoSuchAlgorithmException e) {
            e.printStackTrace();
        }
    }

    public void Move(PlayerDirections moveDirection){
        HistMmorpg.ProtoTravelTo.Builder protoMove = HistMmorpg.ProtoTravelTo.newBuilder();
        protoMove.addTravelVia(moveDirection.toString());
        protoMove.setCharacterID("Char_158");
        clientOperations.Send(protoMove.build().toByteArray());
    }

    public void ArmyStatus(){

    }

    public void Profile(){

    }

    public void Fief(){

    }

    public void Hire(String amount){

    }

    public void Siege(){

    }
}
