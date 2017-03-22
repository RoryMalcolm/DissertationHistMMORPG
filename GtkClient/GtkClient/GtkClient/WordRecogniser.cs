public class WordRecogniser
	{
		public enum Tasks
		{
			Move, Siege, Hire, Fief, Check, ArmyStatus, SyntaxError,
			Exit, Players, Sieges, Profile, SeasonUpdate,
			JournalEntries, Journal, FiefExpenditure
		}

		public Tasks CheckWord(string InputWord)
		{
			InputWord = InputWord.ToUpper();
			switch (InputWord)
			{
			case "MOVE":
				return Tasks.Move;
			case "CHECK":
				return Tasks.Check;
			case "ARMY":
				return Tasks.ArmyStatus;
			case "EXIT":
				return Tasks.Exit;
			case "FIEF":
				return Tasks.Fief;
			case "HIRE":
				return Tasks.Hire;
			case "SIEGE":
				return Tasks.Siege;
			case "PLAYERS":
				return Tasks.Players;
			case "PROFILE":
				return Tasks.Profile;
			case "SUPDATE":
				return Tasks.SeasonUpdate;
			case "SIEGES":
				return Tasks.Sieges;
			case "JOURNALS":
				return Tasks.JournalEntries;
			case "JOURNAL":
				return Tasks.Journal;
			case "NPCS":
				return Tasks.FiefExpenditure;
			default:
				return Tasks.SyntaxError;
			}
		}

		public PlayerOperations.MoveDirections CheckDirections(string InputWord)
		{
			InputWord = InputWord.ToUpper();
			switch (InputWord)
			{
			case "NORTHEAST":
			case "NE":
				return PlayerOperations.MoveDirections.Ne;
			case "NORTHWEST":
			case "NW":
				return PlayerOperations.MoveDirections.Nw;
			case "EAST":
			case "E":
				return PlayerOperations.MoveDirections.E;
			case "WEST":
			case "W":
				return PlayerOperations.MoveDirections.W;
			case "SOUTHWEST":
			case "SW":
				return PlayerOperations.MoveDirections.Sw;
			case "SOUTHEAST":
			case "SE":
				return PlayerOperations.MoveDirections.Se;
			default:
				return PlayerOperations.MoveDirections.SyntaxError;
			}

		}
	}
	
