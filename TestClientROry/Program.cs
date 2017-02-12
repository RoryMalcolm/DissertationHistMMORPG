using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hist_mmorpg;
using System.Threading;

namespace TestClientROry
{
    class Program
    {
        private static WordRecogniser _wordRecogniser;
        private static Server _server;
        private static TextTestClient _testClient;

        private static void Main(string[] args)
        {
            Console.WriteLine("Beginning test run. Enter 'e' to run with encryption");
            bool encrypt = false;
            string userInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(userInput))
            {
                char doEncrypt = userInput[0];
                encrypt = (doEncrypt == 'e');
            }
            var encryptString = encrypt ? "_encrypted_" : "_unencrypted_";
            string datePatern = "MM_dd_H_mm";
            var logFilePath = "TestRun_NoSessions" + encryptString + DateTime.Now.ToString(datePatern) + ".txt";
            using (Globals_Server.LogFile = new System.IO.StreamWriter(logFilePath))
            {
                _wordRecogniser = new WordRecogniser();
                _server = new Server();
                _testClient = new TextTestClient();
            }
            Console.Clear();
            SetUpForDemo();
            LogInPrompt();
            while (_testClient.IsConnectedAndLoggedIn() == false)
            {
                Thread.Sleep(0);
            }
            var command = TokenizeConsoleEntry();
            while (_wordRecogniser.CheckWord(command[0]) != WordRecogniser.Tasks.Exit)
            {
                command = TokenizeConsoleEntry();
            }
            
            Shutdown();
        }

        private static void LogInPrompt()
        {
            Console.WriteLine("Welcome to the JominiEngine Text Client! Please enter your username and password");
            Console.Write("What is your username: ");
            var usernameForReturn = Console.ReadLine();
            Console.Write("What is your password: ");
            var passwordForReturn = Console.ReadLine();
            _testClient.LogInAndConnect(usernameForReturn, passwordForReturn);
            Console.Clear();
        }

        private static void Shutdown()
        {
            _server.Shutdown();
        }

        private static List<string> TokenizeConsoleEntry()
        {
            var command = Console.ReadLine();
            var commands = new List<string>();
            if (command != null)
            {
                foreach (var item in command.Split(' '))
                {
                    commands.Add(item);
                }
            }
            else
            {
                return null;
            }
            return commands;
        }

        
        public static void SetUpForDemo()
        {
            // Make Anselm Marshal very sneaky
            Character Anselm = Globals_Game.getCharFromID("Char_390");
            Character Bishop = Globals_Game.getCharFromID("Char_391");
            Tuple<Trait, int>[] newTraits = new Tuple<Trait, int>[2];
            newTraits[0] = new Tuple<Trait, int>(Globals_Game.traitMasterList["trait_9"], 9);
            newTraits[1] = new Tuple<Trait, int>(Globals_Game.traitMasterList["trait_8"], 9);
            Anselm.traits = newTraits;
            // Make Bishop Henry Marshal not sneaky
            Tuple<Trait, int>[] newTraits2 = new Tuple<Trait, int>[1];
            newTraits2[0] = new Tuple<Trait, int>(Globals_Game.traitMasterList["trait_5"], 2);
            Bishop.traits = newTraits2;
            // Add funds to home treasury
            (Globals_Game.getCharFromID("Char_158") as PlayerCharacter).GetHomeFief().AdjustTreasury(100000);

            // create and add army
            uint[] myArmyTroops1 = new uint[] { 8, 10, 0, 30, 60, 100, 220 };
            Army myArmy1 = new Army(Globals_Game.GetNextArmyID(), Globals_Game.pcMasterList["Char_196"].charID, Globals_Game.pcMasterList["Char_196"].charID, Globals_Game.pcMasterList["Char_196"].days, Globals_Game.pcMasterList["Char_196"].location.id, trp: myArmyTroops1);
            myArmy1.AddArmy();
            // create and add army
            uint[] myArmyTroops2 = new uint[] { 5, 10, 0, 30, 40, 80, 220 };
            Army myArmy2 = new Army(Globals_Game.GetNextArmyID(), Globals_Game.pcMasterList["Char_158"].charID, Globals_Game.pcMasterList["Char_158"].charID, Globals_Game.pcMasterList["Char_158"].days, Globals_Game.pcMasterList["Char_158"].location.id, trp: myArmyTroops2, aggr: 1, odds: 2);
            myArmy2.AddArmy();

            // Add single lady appropriate for marriage
            Nationality nat = Globals_Game.nationalityMasterList["Sco"];
            NonPlayerCharacter proposalChar = new NonPlayerCharacter("Char_626", "Mairi", "Meah", new Tuple<uint, byte>(1162, 3), false, Globals_Game.nationalityMasterList["Sco"], true, 9, 9, new Queue<Fief>(), Globals_Game.languageMasterList["lang_C1"], 90, 9, 9, 9, new Tuple<Trait, int>[0], true, false, "Char_126", null, "Char_126", null, 0, false, false, new List<string>(), null, null, Globals_Game.fiefMasterList["ESW05"]);
            PlayerCharacter pc = Globals_Game.pcMasterList["Char_126"];
            pc.myNPCs.Add(proposalChar);
            proposalChar.inKeep = false;
        }
    }
}
