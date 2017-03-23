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
		Label PlayerLocationLabel;
		Label PlayerLocatiobOutput;
		Label PlayerArmyLabel;
		Label PlayerArmyOutput;
		Label PlayerPurseLabel;
		Label PlayerPurseOutput;

		public ProfileTable (string PlayerID, string PlayerName)
		{
			ProfileLayout = new Table (3, 2, false);
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
		}

	public Table getProfileLayout(){
		return ProfileLayout;
	}
}