using System;
using System.Threading.Tasks;
using hist_mmorpg;

namespace TestClientRory
{
    class PlayerOperations
    {
        public enum MoveDirections
        {
            E, W, Se, Sw, Ne, Nw, SyntaxError
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
        public void ArmyStatus(TextTestClient client)
        {
            ProtoArmy proto = new ProtoArmy();
            proto.ownerID = "Char_158";
            proto.ActionType = Actions.ListArmies;
            client.net.Send(proto);
            var reply = GetActionReply(Actions.ListArmies, client);
            var armies = (ProtoGenericArray<ProtoArmyOverview>) reply.Result;
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Army Report");
            Console.WriteLine("-----------------------------");
            var counter = 0;
            foreach (var army in armies.fields)
            {
                counter++;
                Console.WriteLine("Army " + counter);
                Console.WriteLine("Army ID: " + army.armyID);
                Console.WriteLine("Owner: " + army.ownerName);
                Console.WriteLine("Size: " + army.armySize);
                Console.WriteLine("Location : " + army.locationID);
                Console.WriteLine("-----------------------------");
            }
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

        public void HireTroops(int amount, TextTestClient client)
        {
            ProtoPlayerCharacter protoMessage = new ProtoPlayerCharacter();
            protoMessage.Message = "Char_158";
            protoMessage.ActionType = Actions.ViewChar;
            client.net.Send(protoMessage);
            var armyReply = GetActionReply(Actions.ViewChar, client);
            var armyResult = (ProtoPlayerCharacter)armyReply.Result;
            ProtoRecruit protoRecruit = new ProtoRecruit();
            protoRecruit.ActionType = Actions.RecruitTroops;
            if (amount > 0)
            {
                protoRecruit.amount = (uint) amount;
            }
            protoRecruit.armyID = armyResult.armyID;
            protoRecruit.isConfirm = true;
            client.net.Send(protoRecruit);
            var reply = GetActionReply(Actions.RecruitTroops, client);
            if (reply.Result.ResponseType == DisplayMessages.CharacterRecruitOwn)
            {
                Console.WriteLine("Recruit from a fief you own!");
            }
            else if (reply.Result.ResponseType == DisplayMessages.CharacterRecruitAlready)
            {
                Console.WriteLine("You have already recruited!");
            }
            else if (reply.Result.ResponseType == DisplayMessages.CharacterRecruitInsufficientFunds)
            {
                Console.WriteLine("Insufficient recruitment funds!");
            }
            else
            {
                var result = (ProtoRecruit)reply.Result;
                Console.WriteLine("-----------------------------");
                Console.WriteLine("Recruit Report");
                Console.WriteLine("-----------------------------");
                Console.WriteLine("Army ID: " + result.armyID);
                Console.WriteLine("Recruitment Cost: " + result.cost);
                Console.WriteLine("Amount of Recruits: " + result.amount);
                Console.WriteLine("-----------------------------");
            }
        }

        public void SiegeCurrentFief(TextTestClient client)
        {
            ProtoPlayerCharacter protoMessage = new ProtoPlayerCharacter();
            protoMessage.Message = "Char_158";
            protoMessage.ActionType = Actions.ViewChar;
            client.net.Send(protoMessage);
            var locReply = GetActionReply(Actions.ViewChar, client);
            var locResult = (ProtoPlayerCharacter)locReply.Result;
            ProtoMessage protoSiegeStart = new ProtoMessage();
            protoSiegeStart.ActionType = Actions.BesiegeFief;
            protoSiegeStart.Message = locResult.armyID;
            client.net.Send(protoSiegeStart);
            var reply = GetActionReply(Actions.BesiegeFief, client);
            var result = (ProtoSiegeDisplay) reply.Result;
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Siege Report");
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Besieged Fief: " + result.besiegedFief);
            Console.WriteLine("Besieged Army: " + result.besiegerArmy);
            Console.WriteLine("Siege Successful: " + result.besiegerWon);
            Console.WriteLine("Siege Length: " + result.days + " days");
            Console.WriteLine("Loot Lost: " + result.lootLost);
            Console.WriteLine("-----------------------------");
        }

        public void FiefDetails(TextTestClient client)
        {
            ProtoPlayerCharacter protoMessage = new ProtoPlayerCharacter();
            protoMessage.Message = "Char_158";
            protoMessage.ActionType = Actions.ViewChar;
            client.net.Send(protoMessage);
            var locReply = GetActionReply(Actions.ViewChar, client);
            var locResult = (ProtoPlayerCharacter)locReply.Result;
            ProtoFief protoFief = new ProtoFief();
            protoFief.Message = locResult.location;
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
            var keep = fief.keepLevel;
            Console.WriteLine("Keep Level: "+ keep);
            Console.WriteLine("-----------------------------");
            var militia = fief.militia;
            Console.WriteLine("Number of recruits available: " + militia);
            Console.WriteLine("Number of troops in fief:" + fief.troops);
            Console.WriteLine("-----------------------------");
        }

        public void Players(TextTestClient client)
        {
            ProtoPlayer protoPlayer = new ProtoPlayer();
            protoPlayer.ActionType = Actions.GetPlayers;
            client.net.Send(protoPlayer);
            var reply = GetActionReply(Actions.GetPlayers, client);
            var result = (ProtoGenericArray<ProtoPlayer>) reply.Result;
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Players on Server Report");
            Console.WriteLine("-----------------------------");
            var counter = 0;
            foreach (var player in result.fields)
            {
                counter++;
                Console.WriteLine("Player " + counter);
                Console.WriteLine("Player ID: " + player.playerID);
                Console.WriteLine("Player Name: " + player.pcName);
                Console.WriteLine("-----------------------------");
            }
        }

        public void Profile(TextTestClient client)
        {
            ProtoPlayerCharacter protoMessage = new ProtoPlayerCharacter();
            protoMessage.Message = "Char_158";
            protoMessage.ActionType = Actions.ViewChar;
            client.net.Send(protoMessage);
            var reply = GetActionReply(Actions.ViewChar, client);
            var result = (ProtoPlayerCharacter) reply.Result;
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Player Profile");
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Player ID: " + result.playerID);
            Console.WriteLine("Player Name: " + result.firstName + " " + result.familyName);
            Console.WriteLine("-----------------------------");
            Console.Write("Owned Fiefs: ");
            bool written = false;
            foreach (var fief in result.ownedFiefs)
            {
                if (written == false)
                {
                    Console.Write(fief);
                    written = true;
                }
                else
                    Console.Write(" , " + fief);
            }
            Console.Write("\n");
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Location: " + result.location);
            Console.WriteLine("Army: " + result.armyID);
            Console.WriteLine("Purse: " + result.purse);
            Console.WriteLine("-----------------------------");
        }

        public void SeasonUpdate(TextTestClient client)
        {
            ProtoMessage protoMessage = new ProtoMessage();
            protoMessage.ActionType = Actions.SeasonUpdate;
            client.net.Send(protoMessage);
            var reply = GetActionReply(Actions.SeasonUpdate, client);
            var result = reply.Result;
            Console.WriteLine("Season Updated!");
        }

        public void SiegeList(TextTestClient client)
        {
            ProtoMessage protoMessage = new ProtoMessage();
            protoMessage.ActionType = Actions.SiegeList;
            client.net.Send(protoMessage);
            var reply = GetActionReply(Actions.SiegeList, client);
            var result = (ProtoGenericArray<ProtoSiegeOverview>) reply.Result;
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Sieges Overview");
            Console.WriteLine("-----------------------------");
            foreach (var siege in result.fields)
            {
                Console.Write(siege.siegeID + ": " + siege.besiegingPlayer 
                    + " vs. " + siege.defendingPlayer + " in " + siege.besiegedFief + "\n");
                Console.WriteLine("-----------------------------");
            }
        }

        public void JournalEntries(TextTestClient client)
        {
            ProtoMessage protoMessage = new ProtoMessage();
            protoMessage.ActionType = Actions.ViewJournalEntries;
            client.net.Send(protoMessage);
            var reply = GetActionReply(Actions.ViewJournalEntries, client);
            var result = (ProtoGenericArray<ProtoJournalEntry>) reply.Result;
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Journal Entries");
            Console.WriteLine("-----------------------------");
            if (result.fields != null)
            {
                foreach (var journal in result.fields)
                {
                    Console.WriteLine("-----------------------------");
                    Console.WriteLine("Journal Entry ID: " + journal.jEntryID);
                    Console.WriteLine("Journal Event Year: " + journal.year);
                    Console.WriteLine("Journal Event Location: " + journal.location);
                    Console.WriteLine("Journal Personae: " + journal.personae);
                    Console.WriteLine("-----------------------------");
                }
            }
            else
            {
                Console.WriteLine("No Journal Entries found.");
            }
        }

        public void Journal(string Journal, TextTestClient client){
            ProtoMessage protoMessage = new ProtoMessage();
            protoMessage.ActionType = Actions.ViewJournalEntry;
            protoMessage.Message = Journal;
            client.net.Send(protoMessage);
            var reply = GetActionReply(Actions.ViewJournalEntry, client);
            var journal = (ProtoJournalEntry) reply.Result;
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Journal Entry " + journal.jEntryID);
            Console.WriteLine("-----------------------------");
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Journal Entry ID: " + journal.jEntryID);
            Console.WriteLine("Journal Event Year: " + journal.year);
            Console.WriteLine("Journal Event Location: " + journal.location);
            Console.WriteLine("Journal Personae: " + journal.personae);
            Console.WriteLine("-----------------------------");
        }

        public void AdjustFiefExpenditure(string type, TextTestClient client)
        {
            ProtoNPC protoMessage = new ProtoNPC();
            protoMessage.ActionType = Actions.AdjustExpenditure;
            client.net.Send(protoMessage);
            var reply = GetActionReply(Actions.GetNPCList, client);
            var result = (ProtoGenericArray<ProtoNPC>) reply.Result;
        }
    }
}
