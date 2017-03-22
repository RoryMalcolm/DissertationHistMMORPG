using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestClientRory;
using hist_mmorpg;

namespace ClientDLL
{
    public class MoveResult
    {
        private string FiefId;
        MoveResult(string FiefId)
        {
            this.FiefId = FiefId;
        }

        public string getFiefId()
        {
            return this.FiefId;
        }
    }
    public class PlayerOperationsClassLib
    {
        private readonly TextTestClient _testClient;
        private readonly WordRecogniser _wordRecogniser;
        private readonly PlayerOperations _playerOps;
        
        public PlayerOperationsClassLib()
        {
            _testClient = new TextTestClient();
            _wordRecogniser = new WordRecogniser();
            _playerOps = new PlayerOperations();
            _testClient.LogInAndConnect("helen", "potato");
        }

        public ProtoFief Move(string directions)
        {
            var move = _playerOps.Move(_wordRecogniser.CheckDirections(directions),
                _testClient);

            var moveReturnable = Move(move.fiefID);
            return moveReturnable;
        }

        public ProtoGenericArray<ProtoArmyOverview> ArmyStatus()
        {
            return _playerOps.ArmyStatus(_testClient);
        }

        public ProtoGenericArray<ProtoFief> Check()
        {
            return _playerOps.Check(_testClient);
        }

        public ProtoMessage Hire(string amount)
        {
            return _playerOps.HireTroops(Convert.ToInt32(amount), _testClient);
        }

        public ProtoSiegeDisplay Siege()
        {
            return _playerOps.SiegeCurrentFief(_testClient);
        }

        public ProtoGenericArray<ProtoPlayer> Players()
        {
            return _playerOps.Players(_testClient);
        }

        public ProtoPlayerCharacter Profile()
        { 
            return _playerOps.Profile(_testClient);
        }

        public ProtoMessage SeasonUpdate()
        {
            return _playerOps.SeasonUpdate(_testClient);
        }

        public ProtoGenericArray<ProtoSiegeOverview> Sieges()
        {
            return _playerOps.SiegeList(_testClient);
        }

        public ProtoJournalEntry Journal(string journalForQuery)
        {
            return _playerOps.Journal(journalForQuery, _testClient);
        }

        public ProtoGenericArray<ProtoJournalEntry> JournalEntries()
        {
            return _playerOps.JournalEntries(_testClient);
        }

        public ProtoGenericArray<ProtoNPC> FiefExpenditure(string type)
        {
            return _playerOps.AdjustFiefExpenditure(type, _testClient);
        }
    }
}
