using System;
using Gtk;

public class ProfileTable
	{
		Table ProfileLayout;
		Label PlayerProfile;
		Label PlayerIDLabel;
		Label PlayerIDOutput;
		Label PlayerNameLabel;
		Label PlayerNameOutput;

	public ProfileTable (string PlayerID, string PlayerName, string[] PlayerFiefs, string PlayerLocation, string PlayerArmy, string PlayerPurse)
		{
			uint tableRows = 5;
			ProfileLayout = new Table (4, 2, false);
			PlayerProfile = new Label ("Player Profile");
			PlayerIDLabel = new Label ("Player ID:");
			PlayerIDOutput = new Label (PlayerID);
			PlayerNameLabel = new Label ("Player Name:");
			PlayerNameOutput = new Label (PlayerName);
			ProfileLayout.Attach (PlayerProfile, 0, 2, 0, 1);
			ProfileLayout.Attach (PlayerIDLabel, 0, 1, 1, 2);
			ProfileLayout.Attach (PlayerIDOutput, 1, 2, 1, 2);
			ProfileLayout.Attach (PlayerNameLabel, 0, 1, 2, 3);
			ProfileLayout.Attach (PlayerNameOutput, 1, 2, 2, 3);
			ProfileLayout.Attach(new Label("Owned Fiefs:"),0,2,3,4);
			foreach (var fief in PlayerFiefs) {
				ProfileLayout.Resize (tableRows + 1, 2);
				ProfileLayout.Attach (new Label (fief), 0, 2, tableRows, tableRows + 1);
				tableRows++;
			}
			ProfileLayout.Resize (tableRows + 1, 2);
			ProfileLayout.Attach (new Label ("Location :"), 0, 1, tableRows, tableRows + 1);
			ProfileLayout.Attach (new Label (PlayerLocation), 1, 2, tableRows, tableRows + 1);
			tableRows++;
			ProfileLayout.Resize (tableRows + 1, 2);
			ProfileLayout.Attach (new Label ("Army ID :"), 0, 1, tableRows, tableRows + 1);
			ProfileLayout.Attach (new Label (PlayerArmy), 1, 2, tableRows, tableRows + 1);
			tableRows++;
			ProfileLayout.Resize (tableRows + 1, 2);
			ProfileLayout.Attach (new Label ("Purse :"), 0, 1, tableRows, tableRows + 1);
			ProfileLayout.Attach (new Label (PlayerPurse), 1, 2, tableRows, tableRows + 1);
			tableRows++;
		}

	public Table getProfileLayout(){
		return ProfileLayout;
	}

	public void DestroyTable(){
		ProfileLayout.Destroy ();
	}
}