using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hist_mmorpg;

namespace TestClientRory
{
    class PlayerOperations
    {
        public enum MoveDirections
        {
            North, South, East, West, SyntaxError
        }
        public void Move(MoveDirections directions, TextTestClient client)
        {
            ProtoTravelTo protoTravel = new ProtoTravelTo();
            protoTravel.travelVia = directions.ToString();
            protoTravel.characterID = "helen";
            client.net.Send(protoTravel);
            var reply = GetActionReply(Actions.TravelTo, client);
            var travel = reply.Result.ResponseType;
            Console.WriteLine(travel);
        }

        public void Check(TextTestClient client)
        {
            ProtoMessage checkMessage = new ProtoMessage();
            checkMessage.ActionType = Actions.ViewMyFiefs;
            client.net.Send(checkMessage);
            var reply = GetActionReply(Actions.ViewMyFiefs, client);
            var fiefs = (ProtoGenericArray<ProtoFief>) reply.Result;
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
            ProtoArmyOverview proto = new ProtoArmyOverview();
            proto.ActionType = Actions.ViewArmy;

            var reply = GetActionReply(Actions.ViewArmy, client);
            var army = (ProtoArmyOverview) reply.Result;
            Console.WriteLine(army.ownerName + army.armySize);
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
    }
}
