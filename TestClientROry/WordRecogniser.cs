using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestClientROry
{
    class WordRecogniser
    {
        public enum Tasks
        {
            Move, Pillage, Invade, Check, ArmyStatus, SyntaxError, Exit
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
                case "INVADE":
                    return Tasks.Invade;
                case "CHECK":
                    return Tasks.Check;
                case "ARMY":
                    return Tasks.ArmyStatus;
                case "EXIT":
                    return Tasks.Exit;
                default:
                    return Tasks.SyntaxError;
            }
        }
    }

}
