using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoayleServer
{
	[Serializable]
    public class DataOfAccount
    {
		public string NickName { get; private set; }

		public string Password { get; private set; }

		public long QuantityKills { get; private set; }
        
        public long QuentityDeaths { get; private set; }
      
        public long QuentityBattles { get; private set; }

        public TimeSpan QuentityGameTime { get; private set; }

		public void AddData(DataOfAccount addData)
		{
			QuantityKills += addData.QuantityKills;
			QuentityBattles += addData.QuentityBattles;
			QuentityDeaths += addData.QuentityDeaths;
			QuentityGameTime += addData.QuentityGameTime;
		}

		public DataOfAccount(string nickName, string password, long quantityKills, 
			long quentityDeaths, long quentityBattles, TimeSpan quentityGameTime)
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
