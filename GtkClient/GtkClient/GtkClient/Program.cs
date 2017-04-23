using System;
using GLib;
using Gtk;
using hist_mmorpg;
using Thread = System.Threading.Thread;

public class GtkHelloWorld {
	static PlayerOperations playerOps;
	static TextTestClient client;
	static Button northEast;
	static Button northWest;
	static Button east;
	static Button west;
	static Button southEast;
	static Button southWest;
	static Button fief;
	static Button profile;
	static Button hire;
	static Button siege;
	static Table tableLayout;
	static Window myWin;
	static FiefTable fiefTable;
	static ProfileTable profileTable;
	static Button hireOkayButton;
	static Entry hireTextEntry;
	static Window hireWindow;
    static Window siegeWindow;
    //static PlayerOperationsClassLib playerOps;

    public static void Main() {
		Application.Init();
		LogInWindow login = new LogInWindow ();
		Application.Run();
	}

	public static void LoggedIn(string Username, string Password, object obj, EventArgs args){
		client = new TextTestClient ();
		client.LogInAndConnect (Username, Password);
	    while (client.IsConnectedAndLoggedIn() == false)
	    {
	        Thread.Sleep(0);
	    }
		playerOps = new PlayerOperations();
		myWin = new Window("HistMMorpg Client");
		//Create a label and put some text in it.
		tableLayout = new Table(5,5,false);
		northEast = new Button("North East");
		northWest = new Button("North West");
		east = new Button("East");
		west = new Button("West");
		southEast = new Button("South East");
		southWest = new Button("South West");
		siege = new Button ("Siege");
		hire = new Button ("Hire");
		SetUpDirectionalButtonClicks ();
		SetUpOperationButtonClicks ();
		//Add the label to the form
		tableLayout.Attach(northEast, 0,1,0,1);
		tableLayout.Attach(northWest, 1,2,0,1);
		tableLayout.Attach(east, 0,1,1,2);
		tableLayout.Attach(west,1,2,1,2);
		tableLayout.Attach (siege, 2, 3, 1, 2);
		tableLayout.Attach(southEast, 0,1,2,3);
		tableLayout.Attach(southWest,1,2,2,3);
		tableLayout.Attach(hire,2,3,0,1);
		myWin.Add(tableLayout);

		/*ProtoPlayerCharacter player = playerOps.Profile (client);
		profileTable = new ProfileTable (player.playerID, player.firstName + " " + player.familyName, player.ownedFiefs, player.location, player.armyID, Convert.ToString( player.purse));
		tableLayout.Attach (profileTable.getProfileLayout (), 3, 4, 1, 2);
		ProtoFief fiefData = playerOps.FiefDetails (client);
		fiefTable = new FiefTable (fiefData.fiefID, fiefData.owner, Convert.ToString (fiefData.industry),
			fiefData.charactersInFief, fiefData.armies);
		tableLayout.Attach (fiefTable.getProfileTable (), 3, 4, 2, 3);*/
		//ProfileClickEvent (null, null);
		//FiefClickEvent (null, null);
		//Show Everything
		FiefClickEvent(obj,args);
		ProfileClickEvent (obj, args);
		myWin.ShowAll();
	}

	public static void NorthEastClickEvent(object obj, EventArgs args){
		ProtoFief move = playerOps.Move(PlayerOperations.MoveDirections.Ne, client);
		FiefClickEvent (obj,args);
		ProfileClickEvent (obj, args);
	}

	public static void NorthWestClickEvent(object obj, EventArgs args){
		ProtoFief move = playerOps.Move(PlayerOperations.MoveDirections.Nw, client);
		FiefClickEvent (obj,args);
		ProfileClickEvent (obj, args);
	}

	public static void EastClickEvent(object obj, EventArgs args){
		ProtoFief move = playerOps.Move(PlayerOperations.MoveDirections.E, client);
		FiefClickEvent (obj,args);
		ProfileClickEvent (obj, args);

	}

	public static void WestClickEvent(object obj, EventArgs args){
		ProtoFief move = playerOps.Move(PlayerOperations.MoveDirections.W, client);
		FiefClickEvent (obj,args);
		ProfileClickEvent (obj, args);

	}

	public static void SouthEastClickEvent(object obj, EventArgs args){
		ProtoFief move = playerOps.Move(PlayerOperations.MoveDirections.Se, client);
		FiefClickEvent (obj,args);
		ProfileClickEvent (obj, args);
	}


	public static void SouthWestClickEvent(object obj, EventArgs args){
		ProtoFief move = playerOps.Move(PlayerOperations.MoveDirections.Sw, client);
		FiefClickEvent (obj,args);
		ProfileClickEvent (obj, args);
	}

	public static void ProfileClickEvent(object obj, EventArgs args){
		ProtoPlayerCharacter player = playerOps.Profile (client);
		if (profileTable == null) {
			profileTable = new ProfileTable (player.playerID, player.firstName + " " + player.familyName, player.ownedFiefs, player.location, player.armyID, Convert.ToString( player.purse));
		} else {
			profileTable.DestroyTable ();
			profileTable = new ProfileTable (player.playerID, player.firstName + " " + player.familyName, player.ownedFiefs, player.location, player.armyID, Convert.ToString( player.purse));
		}
		tableLayout.Attach (profileTable.getProfileLayout (), 1, 2, 4, 5);
		myWin.ShowAll ();
	}

	public static void SiegeClickEvent(object obj, EventArgs args){
		var siege = playerOps.SiegeCurrentFief (client);
	    if (siege.GetType() == typeof(ProtoSiegeDisplay))
	    {
	        var siegeDisplay = (ProtoSiegeDisplay) siege;
	        var winner = siegeDisplay.besiegerWon ? siegeDisplay.besiegingPlayer : siegeDisplay.defendingPlayer;
            SiegeResultWindow siegeResultWindow = new SiegeResultWindow(siegeDisplay.besiegingPlayer, siegeDisplay.defendingPlayer, winner);
	        siegeWindow = new Window("Siege Result Window");
	        siegeWindow.Add(siegeResultWindow.getSiegeLayout());
	        siegeWindow.ShowAll();
	    }
	    else
	    {
	        Window errorWindow = new Window("Error Window");
            errorWindow.Add(new Label(siege.Message));
            errorWindow.ShowAll();
	    }

	}

	public static void HireClickEvent(object obj, EventArgs args){
		hireWindow = new Window ("How much?");
		Table gridTable = new Table (2, 1, false);
		hireOkayButton = new Button ("Okay");
		gridTable.Attach (hireOkayButton, 0, 1, 0, 1);
		hireOkayButton.Clicked += HireClickOkayEvent;
		hireTextEntry = new Entry ();
		gridTable.Attach (hireTextEntry, 1, 2, 0, 1);
		hireWindow.Add (gridTable);
		hireWindow.ShowAll ();
	}

	public static void HireClickOkayEvent(object obj, EventArgs args){
		int parsed = -1;
		if (hireTextEntry.Text != "") {
			int.TryParse (hireTextEntry.Text, out parsed);
		}
		if (parsed != -1) {
			playerOps.HireTroops (Convert.ToInt32 (hireTextEntry.Text), client);
		    var armies = playerOps.ArmyStatus(client);
            ProfileClickEvent (obj, args);
		}
		hireWindow.Destroy ();
	}
	public static void FiefClickEvent(object obj, EventArgs args){
		ProtoFief fief = playerOps.FiefDetails (client);
		if (fiefTable == null) {
			fiefTable = new FiefTable (fief.fiefID, fief.owner, Convert.ToString (fief.industry),
				fief.charactersInFief, fief.armies, Convert.ToString(fief.militia));
		} else {
			fiefTable.destroyTable ();

			fiefTable = new FiefTable (fief.fiefID, fief.owner, Convert.ToString (fief.industry),
				fief.charactersInFief, fief.armies, Convert.ToString(fief.militia));
		}
		tableLayout.Attach (fiefTable.getProfileTable (), 0, 1, 4, 5);
		myWin.ShowAll ();
	}

    public static void FiefClickEvent(object obj, EventArgs args, uint ArmySize)
    {
        ProtoFief fief = playerOps.FiefDetails(client);
        if (fiefTable == null)
        {
            fiefTable = new FiefTable(fief.fiefID, fief.owner, Convert.ToString(fief.industry),
                fief.charactersInFief, fief.armies, Convert.ToString(fief.militia));
        }
        else
        {
            fiefTable.destroyTable();

            fiefTable = new FiefTable(fief.fiefID, fief.owner, Convert.ToString(fief.industry),
                fief.charactersInFief, fief.armies, Convert.ToString(fief.militia));
        }
        tableLayout.Attach(fiefTable.getProfileTable(), 0, 1, 4, 5);
        myWin.ShowAll();
    }



    public static void SetUpDirectionalButtonClicks(){
		northEast.Clicked += NorthEastClickEvent;
		northWest.Clicked += NorthWestClickEvent;
		east.Clicked += EastClickEvent;
		west.Clicked += WestClickEvent;
		southEast.Clicked += SouthEastClickEvent;
		southWest.Clicked += SouthWestClickEvent;
	}

	public static void SetUpOperationButtonClicks(){
		siege.Clicked += SiegeClickEvent;
		hire.Clicked += HireClickEvent;
	}
}