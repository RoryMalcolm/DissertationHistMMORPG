﻿namespace TestClientRory
{
    class WordRecogniser
    {
        public enum Tasks
        {
            Move, Pillage, Siege, Hire, Fief, Check, ArmyStatus, SyntaxError,
            Exit, Players, Sieges, Profile, SeasonUpdate,
            JournalEntries, Journal
        }

        public Tasks CheckWord(string InputWord)
        {
            InputWord = InputWord.ToUpper();
            switch (InputWord)
            {
                case "MOVE":
                    return Tasks.Move;
                case "PILLAGE":
                    return Tasks.Pillage;
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
                    return PlayerOperations.MoveDirections.NE;
                case "NORTHWEST":
                case "NW":
                    return PlayerOperations.MoveDirections.NW;
                case "EAST":
                case "E":
                    return PlayerOperations.MoveDirections.E;
                case "WEST":
                case "W":
                    return PlayerOperations.MoveDirections.W;
                case "SOUTHWEST":
                case "SW":
                    return PlayerOperations.MoveDirections.SW;
                case "SOUTHEAST":
                case "SE":
                    return PlayerOperations.MoveDirections.SE;
                default:
                    return PlayerOperations.MoveDirections.SyntaxError;
            }

        }
    }

}
