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
        private static Server _server;
        private static Game _game;
        private static TextTestClient _testClient;
        private static PlayerCharacter myPlayer;

        private static void Main(string[] args)
        {
            var encryptString = "_encrypted_";
            string datePatern = "MM_dd_H_mm";
            var logFilePath = "TestRun_NoSessions" + encryptString + DateTime.Now.ToString(datePatern) + ".txt";
            TextTestClient client = new TextTestClient();
            Globals_Game.pcMasterList.Add("rory", new PlayerCharacter());
            using (Globals_Server.LogFile = new System.IO.StreamWriter(logFilePath))
            {
                //_game = new Game();
                _wordRecogniser = new WordRecogniser();
                //_server = new Server();
                _testClient = new TextTestClient();

                Console.Clear();
                LogInPrompt();
                while (_testClient.IsConnectedAndLoggedIn() == false)
                {
                    Thread.Sleep(0);
                }
                var command = TokenizeConsoleEntry();
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

        public static void ProcessCommand(WordRecogniser.Tasks task, List<String> arguments)
        {
            WordRecogniser wordRecogniser = new WordRecogniser();
            PlayerOperations player = new PlayerOperations();
            switch (task)
            {
                case WordRecogniser.Tasks.ArmyStatus:
                        player.ArmyStatus();
                    break;
                case WordRecogniser.Tasks.Check:
                        player.Check(wordRecogniser.CheckDirections(arguments[1]));
                    break;
                case WordRecogniser.Tasks.Invade:
                    player.Invade(wordRecogniser.CheckDirections(arguments[1]));
                    break;
                case WordRecogniser.Tasks.Move:
                    player.Move(wordRecogniser.CheckDirections(arguments[1]));
                    break;
                case WordRecogniser.Tasks.Pillage:
                    player.Pillage(wordRecogniser.CheckDirections(arguments[1]));
                    break;
                case WordRecogniser.Tasks.Exit:
                    break;
            }
        }
    }
}
