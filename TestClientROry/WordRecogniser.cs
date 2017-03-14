using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace TestClientRory
{
    class WordRecogniser
    {
        public enum Tasks
        {
            Move, Pillage, Siege, Hire, Fief, Check, ArmyStatus, SyntaxError, Exit, Players, Profile, SeasonUpdate
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

}
