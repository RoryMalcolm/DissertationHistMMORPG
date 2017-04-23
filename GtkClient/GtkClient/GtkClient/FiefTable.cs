using System;
using Gtk;
using hist_mmorpg;
public class FiefTable
	{
		Table ProfileLayout;
		Label PlayerProfile;
		Label OwnerLabel;
		Label OwnerOutput;
		Label IndustryLevelLabel;
		Label IndustryLevelOutput;
		Label FiefIDLabel;
		Label FiefIDOuput;
        uint tableRows;
	public FiefTable (string FiefID, string Owner, string IndustryLevel, ProtoCharacterOverview[] chars,
		ProtoArmyOverview[] armys, string militia)
		{
            tableRows =5;
			ProfileLayout = new Table (5, 2, false);
			PlayerProfile = new Label ("Fief Profile");
            FiefIDLabel = new Label ("Fief ID:");
            FiefIDOuput = new Label (FiefID);
			OwnerLabel = new Label ("Owner:");
			OwnerOutput = new Label (Owner);
			IndustryLevelLabel = new Label ("Industry Level:");
			IndustryLevelOutput = new Label (IndustryLevel);
			ProfileLayout.Attach (PlayerProfile, 0, 2, 0, 1);
            ProfileLayout.Attach (FiefIDLabel, 0,1,1,2);
            ProfileLayout.Attach (FiefIDOuput, 1,2,1,2);
			ProfileLayout.Attach (OwnerLabel, 0, 1, 2, 3);
			ProfileLayout.Attach (OwnerOutput, 1, 2, 2, 3);
			ProfileLayout.Attach (IndustryLevelLabel, 0, 1, 3, 4);
			ProfileLayout.Attach (IndustryLevelOutput, 1, 2, 3, 4);
			ProfileLayout.Attach (new Label ("Number of Troops for Hire: "), 0, 1, 4, 5);
			ProfileLayout.Attach (new Label (militia), 1, 2, 4, 5);
            foreach(var character in chars){
                tableRows = tableRows+1;
                ProfileLayout.Resize(tableRows, 2);
                ProfileLayout.Attach(new Label ("Char ID: "), 0,1,tableRows-1,tableRows);
                ProfileLayout.Attach(new Label (character.charID), 1,2,tableRows-1,tableRows);
                tableRows = tableRows+1;
                ProfileLayout.Resize(tableRows, 2);
                ProfileLayout.Attach(new Label ("Name: "), 0,1,tableRows-1,tableRows);
                ProfileLayout.Attach(new Label (character.charName), 1,2,tableRows-1,tableRows);
                tableRows = tableRows+1;
                ProfileLayout.Resize(tableRows, 2);
                ProfileLayout.Attach(new Label ("Role: "), 0,1,tableRows-1,tableRows);
                ProfileLayout.Attach(new Label (character.role), 1,2,tableRows-1,tableRows);
            }
            foreach(var army in armys){
                tableRows = tableRows+1;
                ProfileLayout.Resize(tableRows, 2);
                ProfileLayout.Attach(new Label ("Army ID: "), 0,1,tableRows-1,tableRows);
                ProfileLayout.Attach(new Label (army.armyID), 1,2,tableRows-1,tableRows);
                tableRows = tableRows+1;
                ProfileLayout.Resize(tableRows, 2);
                ProfileLayout.Attach(new Label ("Size: "), 0,1,tableRows-1,tableRows);
				ProfileLayout.Attach(new Label (Convert.ToString(army.armySize)), 1,2,tableRows-1,tableRows);
                tableRows = tableRows+1;
                ProfileLayout.Resize(tableRows, 2);
                ProfileLayout.Attach(new Label ("Leader: "), 0,1,tableRows-1,tableRows);
                ProfileLayout.Attach(new Label (army.leaderName), 1,2,tableRows-1,tableRows);
                tableRows = tableRows+1;
                ProfileLayout.Resize(tableRows, 2);
                ProfileLayout.Attach(new Label ("Owner: "), 0,1,tableRows-1,tableRows);
				ProfileLayout.Attach(new Label (army.ownerName), 1,2,tableRows-1,tableRows);
            }
		}



	public Table getProfileTable(){
		return ProfileLayout;
	}

	public void destroyTable(){
		ProfileLayout.Destroy ();
	}
}