using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestClientROry
{
    class LocalPlayerInfo
    {
        private string _id;
        private string _armyId;
        private string _location;

        public LocalPlayerInfo()
        {
            
        }
        public LocalPlayerInfo(string id, string location)
        {
            _id = id;
            _location = location;
        }

        public string GetId()
        {
            return _id;
        }

        public string GetArmyId()
        {
            return _armyId;
        }

        public string GetLocation()
        {
            return _location;
        }

        public void SetId(string idForSet)
        {
            _id = idForSet;
        }

        public void SetArmyId(string armyIdForSet)
        {
            _armyId = armyIdForSet;
        }

        public void SetLocation(string locationForSet)
        {
            _location = locationForSet;
        }
    }
}
