using System;
using Gtk;

public class GtkHelloWorld {
  public static void Main() {
     Application.Init();
 
     //Create the Window
     Window myWin = new Window("My first GTK# Application! ");
     myWin.Resize(200,200);
 
     //Create a label and put some text in it.
     Table tableLayout = new Table(4,2,false);
     Button northEast = new Button("North East");
     Button northWest = new Button("North West");
     Button east = new Button("East");
     Button west = new Button("West");
     Button southEast = new Button("South East");
     Button southWest = new Button("South West");

     Label currentUserLabel = new Label("Current User:");
     Label currentUserOutput = new Label("");
     //Add the label to the form
     tableLayout.Attach(northEast, 0,1,0,1);
     tableLayout.Attach(northWest, 1,2,0,1);
     tableLayout.Attach(east, 0,1,1,2);
     tableLayout.Attach(west,1,2,1,2);
     tableLayout.Attach(southEast, 0,1,2,3);
     tableLayout.Attach(southWest,1,2,2,3);
     tableLayout.Attach(currentUserLabel, 0,1,3,4);
     tableLayout.Attach(currentUserOutput,1,2,3,4);
     myWin.Add(tableLayout);

     //Show Everything
     myWin.ShowAll();
 
     Application.Run();
   }

   public static void ButtonPressHandler(object obj, ButtonPressEventArgs args){

   }
 }
