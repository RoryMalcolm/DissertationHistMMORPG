using System;
using Gtk;
using hist_mmorpg;
using ClientDLL;

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
	//static PlayerOperationsClassLib playerOps;

	static Label currentUserOutput;

	public static void Main() {
		client = new TextTestClient ();
		client.LogInAndConnect ("helen", "potato");
		playerOps = new PlayerOperations();
		//playerOps = new PlayerOperationsClassLib();
		Application.Init();
		//Create the Window
		myWin = new Window("HistMMorpg Client");
		myWin.Resize(1000,1000);

		//Create a label and put some text in it.
		tableLayout = new Table(4,4,false);
		northEast = new Button("North East");
		northWest = new Button("North West");
		east = new Button("East");
		west = new Button("West");
		southEast = new Button("South East");
		southWest = new Button("South West");
		fief = new Button ("Fief");
		profile = new Button ("Profile");
		siege = new Button ("Siege");
		hire = new Button ("Hire");
		SetUpDirectionalButtonClicks ();
		SetUpOperationButtonClicks ();
		Label currentUserLabel = new Label("Current User:");
		currentUserOutput = new Label("");
		//Add the label to the form
		tableLayout.Attach(northEast, 0,1,0,1);
		tableLayout.Attach(northWest, 1,2,0,1);
		tableLayout.Attach (profile, 2, 3, 0, 1);
		tableLayout.Attach (fief, 3, 4, 0, 1);
		tableLayout.Attach(east, 0,1,1,2);
		tableLayout.Attach(west,1,2,1,2);
		tableLayout.Attach (siege, 2, 3, 1, 2);
		tableLayout.Attach(southEast, 0,1,2,3);
		tableLayout.Attach(southWest,1,2,2,3);
		tableLayout.Attach(hire,2,3,2,3);
		tableLayout.Attach(currentUserLabel, 0,1,3,4);
		tableLayout.Attach(currentUserOutput,1,2,3,4);
		myWin.Add(tableLayout);

		//Show Everything
		myWin.ShowAll();

		Application.Run();
	}

	public static void NorthEastClickEvent(object obj, EventArgs args){
		ProtoFief move = playerOps.Move(PlayerOperations.MoveDirections.Ne, client);
		currentUserOutput.Text = move.fiefID;
		FiefClickEvent (obj,args);
	}

	public static void NorthWestClickEvent(object obj, EventArgs args){
		ProtoFief move = playerOps.Move(PlayerOperations.MoveDirections.Nw, client);
		currentUserOutput.Text = move.fiefID;
		FiefClickEvent (obj,args);
	}

	public static void EastClickEvent(object obj, EventArgs args){
		ProtoFief move = playerOps.Move(PlayerOperations.MoveDirections.E, client);
		currentUserOutput.Text = move.fiefID;
		FiefClickEvent (obj,args);
	}

	public static void WestClickEvent(object obj, EventArgs args){
		ProtoFief move = playerOps.Move(PlayerOperations.MoveDirections.W, client);
		currentUserOutput.Text = move.fiefID;
		FiefClickEvent (obj,args);
	}

	public static void SouthEastClickEvent(object obj, EventArgs args){
		ProtoFief move = playerOps.Move(PlayerOperations.MoveDirections.Se, client);
		currentUserOutput.Text = move.fiefID;
		FiefClickEvent (obj,args);
	}


	public static void SouthWestClickEvent(object obj, EventArgs args){
		ProtoFief move = playerOps.Move(PlayerOperations.MoveDirections.Sw, client);
		currentUserOutput.Text = move.fiefID;
		FiefClickEvent (obj,args);
	}

	public static void ProfileClickEvent(object obj, EventArgs args){
		ProtoPlayerCharacter player = playerOps.Profile (client);
		ProfileTable profileTable = new ProfileTable (player.playerID, player.firstName + " " + player.familyName);
		Table forAdd = profileTable.getProfileLayout ();
		tableLayout.Remove (forAdd);
		tableLayout.Attach (forAdd, 3, 4, 1, 2);
		myWin.ShowAll ();
	}

	public static void SiegeClickEvent(object obj, EventArgs args){
		ProtoSiegeDisplay siege = playerOps.SiegeCurrentFief (client);
	}

	public static void HireClickEvent(object obj, EventArgs args){
	}

	public static void FiefClickEvent(object obj, EventArgs args){
		ProtoFief fief = playerOps.FiefDetails (client);
		FiefTable fiefTable = new FiefTable (fief.fiefID, fief.owner, Convert.ToString(fief.industry),
			fief.charactersInFief, fief.armies);
		tableLayout.Attach (fiefTable.getProfileTable(), 3, 4, 2, 3);
		myWin.ShowAll ();
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
		profile.Clicked += ProfileClickEvent;
		siege.Clicked += SiegeClickEvent;
		hire.Clicked += HireClickEvent;
		fief.Clicked += FiefClickEvent;
	}
}