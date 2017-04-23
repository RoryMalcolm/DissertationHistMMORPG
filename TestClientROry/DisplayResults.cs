using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hist_mmorpg;

namespace TestClientROry
{
    class DisplayResults
    {
        public void DisplayMove(ProtoFief moveProtoFief)
        {
            Console.WriteLine("New Fief ID: " + moveProtoFief.fiefID);
        }

        public void DisplayArmyStatus(ProtoGenericArray<ProtoArmyOverview> armiesProtoBuf)
        {
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Army Report");
            Console.WriteLine("-----------------------------");
            var counter = 0;
            foreach (var army in armiesProtoBuf.fields)
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

        public void DisplayCheck(ProtoGenericArray<ProtoFief> fiefsProtoBuf)
        {
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Fiefs Owned Report");
            Console.WriteLine("-----------------------------");
            Console.Write("Fiefs owned by ");
            bool written = false;
            foreach (var fief in fiefsProtoBuf.fields)
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

        public void DisplayHire(ProtoMessage recruitProtoBuf)
        {
            if (recruitProtoBuf.ResponseType == DisplayMessages.CharacterRecruitOwn)
            {
                Console.WriteLine("Recruit from a fief you own!");
            }
            else if (recruitProtoBuf.ResponseType == DisplayMessages.CharacterRecruitAlready)
            {
                Console.WriteLine("You have already recruited!");
            }
            else if (recruitProtoBuf.ResponseType == DisplayMessages.CharacterRecruitInsufficientFunds)
            {
                Console.WriteLine("Insufficient recruitment funds!");
            }
            else
            {
                var recruitProtoBufCast = (ProtoRecruit) recruitProtoBuf;
                Console.WriteLine("-----------------------------");
                Console.WriteLine("Recruit Report");
                Console.WriteLine("-----------------------------");
                Console.WriteLine("Army ID: " + recruitProtoBufCast.armyID);
                Console.WriteLine("Recruitment Cost: " + recruitProtoBufCast.cost);
                Console.WriteLine("Amount of Recruits: " + recruitProtoBufCast.amount);
                Console.WriteLine("-----------------------------");
            }
        }

        public void Help()
        {
            Console.WriteLine("Command List: \n siege\n army \n check \n profile \n fief \n move [direction parameter ne,nw,e,w,se,sw] \n hire [amount] \n fiefs");
        }

        public void DisplaySiege(ProtoSiegeDisplay siegeDisplayProtoBuf)
        {
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Siege Report");
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Besieged Fief: " + siegeDisplayProtoBuf.besiegedFief);
            Console.WriteLine("Besieged Army: " + siegeDisplayProtoBuf.besiegerArmy);
            Console.WriteLine("Siege Successful: " + siegeDisplayProtoBuf.besiegerWon);
            Console.WriteLine("Siege Length: " + siegeDisplayProtoBuf.days + " days");
            Console.WriteLine("Loot Lost: " + siegeDisplayProtoBuf.lootLost);
            Console.WriteLine("-----------------------------");
        }

        public void DisplayFief(ProtoFief fiefProtoBuf)
        {
            var armys = fiefProtoBuf.armies;
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Fief Report");
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Fief ID: " + fiefProtoBuf.fiefID);
            Console.WriteLine("Owner: " + fiefProtoBuf.owner);
            Console.WriteLine("Owner ID: " + fiefProtoBuf.ownerID);
            Console.WriteLine("Industry Level: " + fiefProtoBuf.industry);
            var characters = fiefProtoBuf.charactersInFief;
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
            var keep = fiefProtoBuf.keepLevel;
            Console.WriteLine("Keep Level: " + keep);
            Console.WriteLine("-----------------------------");
            var militia = fiefProtoBuf.militia;
            Console.WriteLine("Number of recruits available: " + militia);
            Console.WriteLine("Number of troops in fief: " + fiefProtoBuf.troops);
            Console.WriteLine("-----------------------------");
        }

        public void DisplayPlayers(ProtoGenericArray<ProtoPlayer> playersProtoBuf)
        {
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Players on Server Report");
            Console.WriteLine("-----------------------------");
            var counter = 0;
            foreach (var player in playersProtoBuf.fields)
            {
                counter++;
                Console.WriteLine("Player " + counter);
                Console.WriteLine("Player ID: " + player.playerID);
                Console.WriteLine("Player Name: " + player.pcName);
                Console.WriteLine("-----------------------------");
            }
        }

        public void DisplayProfile(ProtoPlayerCharacter characterProtoBuf)
        {
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Player Profile");
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Player ID: " + characterProtoBuf.playerID);
            Console.WriteLine("Player Name: " + characterProtoBuf.firstName + " " + characterProtoBuf.familyName);
            Console.WriteLine("-----------------------------");
            Console.Write("Owned Fiefs: ");
            bool written = false;
            foreach (var fief in characterProtoBuf.ownedFiefs)
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
            Console.WriteLine("Location: " + characterProtoBuf.location);
            Console.WriteLine("Army: " + characterProtoBuf.armyID);
            Console.WriteLine("Purse: " + characterProtoBuf.purse);
            Console.WriteLine("-----------------------------");
        }

        public void DisplaySeasonUpdate()
        {
            Console.WriteLine("Season Updated!");
        }

        public void DisplaySiegeResult(ProtoGenericArray<ProtoSiegeOverview> siegeResultProtobuf)
        {
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Sieges Overview");
            Console.WriteLine("-----------------------------");
            foreach (var siege in siegeResultProtobuf.fields)
            {
                Console.Write(siege.siegeID + ": " + siege.besiegingPlayer
                    + " vs. " + siege.defendingPlayer + " in " + siege.besiegedFief + "\n");
                Console.WriteLine("-----------------------------");
            }
        }

        public void DisplayJournalEntries(ProtoGenericArray<ProtoJournalEntry> journalEntriesProtoBuf)
        {
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Journal Entries");
            Console.WriteLine("-----------------------------");
            if (journalEntriesProtoBuf.fields != null)
            {
                foreach (var journal in journalEntriesProtoBuf.fields)
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

        public void DisplayJournalEntry(ProtoJournalEntry journalEntryProtoBuf)
        {
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Journal Entry " + journalEntryProtoBuf.jEntryID);
            Console.WriteLine("-----------------------------");
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Journal Entry ID: " + journalEntryProtoBuf.jEntryID);
            Console.WriteLine("Journal Event Year: " + journalEntryProtoBuf.year);
            Console.WriteLine("Journal Event Location: " + journalEntryProtoBuf.location);
            Console.WriteLine("Journal Personae: " + journalEntryProtoBuf.personae);
            Console.WriteLine("-----------------------------");
        }
    }
}
