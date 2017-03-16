using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestClientRory;
using hist_mmorpg;

namespace ClientDLL
{
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
        }

        public ProtoFief Move(string directions)
        {
            return _playerOps.Move(_wordRecogniser.CheckDirections(directions),
                _testClient);
        }

        public void ArmyStatus()
        {
            _playerOps.ArmyStatus(_testClient);
        }

        public void Check()
        {
            _playerOps.Check(_testClient);
        }

        public void Hire(string amount)
        {
            _playerOps.HireTroops(Convert.ToInt32(amount), _testClient);
        }

        public void Siege()
        {
            _playerOps.SiegeCurrentFief(_testClient);
        }

        public void Players()
        {
            _playerOps.Players(_testClient);
        }

        public void Profile()
        {
            _playerOps.Profile(_testClient);
        }

        public void SeasonUpdate()
        {
            _playerOps.SeasonUpdate(_testClient);
        }

        public void Sieges()
        {
            _playerOps.SiegeList(_testClient);
        }

        public void Journal(string journalForQuery)
        {
            _playerOps.Journal(journalForQuery, _testClient);
        }

        public void JournalEntries()
        {
            _playerOps.JournalEntries(_testClient);
        }

        public void FiefExpenditure(string type)
        {
            _playerOps.AdjustFiefExpenditure(type, _testClient);
        }
    }
}
