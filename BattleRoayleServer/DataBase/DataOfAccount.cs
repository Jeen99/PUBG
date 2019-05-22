using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoayleServer
{
    class DataOfAccount
    {
        public long QuantityKills { get; private set; }
        
        public long QuentityDeaths { get; private set; }
      
        public long QuentityBattles { get; private set; }

        public DateTime QuentityGameTime { get; private set; }

        public DataOfAccount(long quantityKills, long quentityDeaths, long quentityBattles, DateTime quentityGameTime)
        {
            QuantityKills = quantityKills;
            QuentityDeaths = quentityDeaths;
            QuentityBattles = quentityBattles;
            QuentityGameTime = quentityGameTime;
        }
    }
}
