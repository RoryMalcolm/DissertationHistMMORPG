using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hist_mmorpg;
using TestClientROry;

namespace TestClientRory
{
    class PlayerOperations
    {
        public enum MoveDirections
        {
            E, W, SE, SW, NE, NW, SyntaxError
        }
        public void Move(MoveDirections directions, TextTestClient client)
        {
            ProtoTravelTo protoTravel = new ProtoTravelTo();
            protoTravel.travelVia = new[] {directions.ToString()};
            protoTravel.characterID = "Char_158";
            client.net.Send(protoTravel);
            var reply = GetActionReply(Actions.TravelTo, client);
            var travel = (ProtoFief) reply.Result;
            Console.WriteLine("New Fief ID: " + travel.fiefID);
        }

        public void Check(TextTestClient client)
        {
            ProtoMessage checkMessage = new ProtoMessage();
            checkMessage.ActionType = Actions.ViewMyFiefs;
            client.net.Send(checkMessage);
            var reply = GetActionReply(Actions.ViewMyFiefs, client);
            var fiefs = (ProtoGenericArray<ProtoFief>) reply.Result;
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Fiefs Owned Report");
            Console.WriteLine("-----------------------------");
            Console.Write("Fiefs owned by " );
            bool written = false;
            foreach (var fief in fiefs.fields)
            {
                if (!written)
                {
                    Console.Write(fief.owner + ": \n");
                    written = true;
                }
                Console.WriteLine(fief.fiefID);
            }
            Console.WriteLine("-----------------------------");
        }

        public void Pillage(MoveDirections directions, TextTestClient client)
        {
            ProtoMessage siegeMessage = new ProtoMessage();
            siegeMessage.ActionType = Actions.BesiegeFief;
            client.net.Send(siegeMessage);
            var reply = GetActionReply(Actions.BesiegeFief, client);
            var siege = reply.Result.ResponseType;
            Console.WriteLine(siege);
        }

        public void ArmyStatus(TextTestClient client)
        {
            ProtoArmy proto = new ProtoArmy();
            proto.ownerID = "helen";
            proto.ActionType = Actions.ViewArmy;
            client.net.Send(proto);
            var reply = GetActionReply(Actions.ViewArmy, client);
            var army = (ProtoArmy) reply.Result;
            Console.WriteLine(army.ownerID + army.armyID);
        }

        public Task<ProtoMessage> GetActionReply(Actions action, TextTestClient client)
        {
            Task<ProtoMessage> responseTask = client.GetReply();
            responseTask.Wait();
            while (responseTask.Result.ActionType != action)
            {
                responseTask = client.GetReply();
                responseTask.Wait();
            }
            client.ClearMessageQueues();
            return responseTask;
        }

        public void HireTroops(int amount, string armyID, TextTestClient client)
        {
            ProtoRecruit protoRecruit = new ProtoRecruit();
            if (amount > 0)
            {
                protoRecruit.amount = (uint) amount;
            }
            protoRecruit.armyID = armyID;
            protoRecruit.isConfirm = true;
            client.net.Send(protoRecruit);
            var reply = GetActionReply(Actions.RecruitTroops, client);
            var result = reply.Result;
        }

        public void FiefDetails(TextTestClient client)
        {
            ProtoFief protoFief = new ProtoFief();
            protoFief.ActionType = Actions.ViewFief;
            client.net.Send(protoFief);
            var reply = GetActionReply(Actions.ViewFief, client);
            var fief = (ProtoFief) reply.Result;
            var armys = fief.armies;
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Fief Report");
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Fief ID: "+ fief.fiefID);
            Console.WriteLine("Owner: " + fief.owner);
            Console.WriteLine("Owner ID: " + fief.ownerID);
            Console.WriteLine("Industry Level: " + fief.industry);
            var characters = fief.charactersInFief;
            Console.WriteLine("Characters in Fief: ");
            foreach (var character in characters)
            {
                Console.WriteLine("-----------------------------");
                Console.WriteLine("ID: " + character.charID);
                Console.WriteLine("Name :" + character.charName);
                Console.WriteLine("Role: " + character.role);
            }
            Console.WriteLine("-----------------------------");
            if (armys != null)
            {
                Console.WriteLine("Armies in Fief: ");
                foreach (var army in armys)
                {
                    Console.WriteLine("-----------------------------");
                    Console.WriteLine("ID: " + army.armyID);
                    Console.WriteLine("Size :" + army.armySize);
                    Console.WriteLine("Leader: " + army.leaderName);
                    Console.WriteLine("Owner: " + army.ownerName);
                }
                Console.WriteLine("-----------------------------");
            }
            var militia = fief.militia;
            Console.WriteLine("Number of recruits available: " + militia);
            Console.WriteLine("Number of troops in fief:" + fief.troops);
            Console.WriteLine("-----------------------------");
        }
    }
}
