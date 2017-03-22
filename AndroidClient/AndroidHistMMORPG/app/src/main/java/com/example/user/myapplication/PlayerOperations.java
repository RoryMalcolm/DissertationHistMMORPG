package com.example.user.myapplication;

/**
 * Created by User on 19/03/2017.
 */

public class PlayerOperations {

    ClientOperations clientOperations;
    public enum PlayerDirections{
        NE, NW, W, E, SE, SW
    }

    public PlayerOperations(){
        clientOperations = new ClientOperations();
        clientOperations.Connect("helen", "potato");
    }

    public void Move(PlayerDirections moveDirection){

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
