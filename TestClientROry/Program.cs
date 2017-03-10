using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hist_mmorpg;
using System.Threading;

namespace TestClientRory
{
    class Program
    {
        private static WordRecogniser _wordRecogniser;
        private static TextTestClient _testClient;

        private static void Main(string[] args)
        {
            var encryptString = "_encrypted_";
            string datePatern = "MM_dd_H_mm";
            var logFilePath = "TestRun_NoSessions" + encryptString + DateTime.Now.ToString(datePatern) + ".txt";
            TextTestClient client = new TextTestClient();
            Globals_Game.pcMasterList.Add("rory", new PlayerCharacter());
            using (Globals_Server.LogFile = new System.IO.StreamWriter(logFilePath))
            {
                _wordRecogniser = new WordRecogniser();
                _testClient = new TextTestClient();

                Console.Clear();
                LogInPrompt();
                while (_testClient.IsConnectedAndLoggedIn() == false)
                {
                    Thread.Sleep(0);
                }
                var command = TokenizeConsoleEntry();
                ProcessCommand(_wordRecogniser.CheckWord(command[0]), command);
                while (_wordRecogniser.CheckWord(command[0]) != WordRecogniser.Tasks.Exit)
                {
                    command = TokenizeConsoleEntry();
                    ProcessCommand(_wordRecogniser.CheckWord(command[0]), command);
                }

                Shutdown();
            }
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
            _testClient.LogOut();
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

        public static void ProcessCommand(WordRecogniser.Tasks task, List<String> arguments)
        {
            WordRecogniser wordRecogniser = new WordRecogniser();
            PlayerOperations player = new PlayerOperations();
            switch (task)
            {
                case WordRecogniser.Tasks.ArmyStatus:
                    player.ArmyStatus(_testClient);
                    break;
                case WordRecogniser.Tasks.Check:
                    player.Check(_testClient);
                    break;
                case WordRecogniser.Tasks.Fief:
                    player.FiefDetails(_testClient);
                    break;
                case WordRecogniser.Tasks.Move:
                    if (ValidateArgs(arguments))
                    {
                        player.Move(wordRecogniser.CheckDirections(arguments[1]), _testClient);
                    }
                    else
                    {
                        SyntaxError();
                    }
                    break;
                case WordRecogniser.Tasks.Hire:
                    if (ValidateArgs(arguments))
                    {
                        player.HireTroops(Convert.ToInt32(arguments[1]), "Army_" + arguments[2], _testClient);
                    }
                    break;
                case WordRecogniser.Tasks.Pillage:
                    if (ValidateArgs(arguments))
                    {
                        player.Pillage(wordRecogniser.CheckDirections("Army_" + arguments[1]), _testClient);
                    }
                    else
                    {
                        SyntaxError();
                    }
                    break;
                case WordRecogniser.Tasks.Siege:
                    if (ValidateArgs(arguments))
                    {
                        player.SiegeCurrentFief(arguments[1], _testClient);
                    }
                    else
                    {
                        SyntaxError();
                    }
                    break;
                case WordRecogniser.Tasks.Players:
                    player.Players(_testClient);
                    break;
                case WordRecogniser.Tasks.Profile: 
                    player.Profile(_testClient);
                    break;
                case WordRecogniser.Tasks.SeasonUpdate:
                    player.SeasonUpdate(_testClient);
                    break;
                case WordRecogniser.Tasks.Exit:
                    break;
            }
        }

        static bool ValidateArgs(List<String> argumentList)
        {
            var counter =0;
            foreach (var argument in argumentList)
            {
                counter++;
            }
            if (counter >= 2)
            {
                return true;
            }
            return false;
        }

        static void SyntaxError()
        {
            Console.WriteLine("Syntax Error: No argument provided");
        }
    }
}
