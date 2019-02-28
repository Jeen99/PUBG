using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoayleServer
{
    public class DataOfAccount
    {
		public string NickName { get; private set; }

		public string Password { get; private set; }

		public long QuantityKills { get; private set; }
        
        public long QuentityDeaths { get; private set; }
      
        public long QuentityBattles { get; private set; }

        public DateTime QuentityGameTime { get; private set; }

		public DataOfAccount(string nickName, string password, long quantityKills, 
			long quentityDeaths, long quentityBattles, DateTime quentityGameTime)
		{
			NickName = nickName;
			Password = password;
			QuantityKills = quantityKills;
			QuentityDeaths = quentityDeaths;
			QuentityBattles = quentityBattles;
			QuentityGameTime = quentityGameTime;
		}
	}
}
