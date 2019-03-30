using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyalClient
{
	class AccountModel:IAccountModel
	{
		public void CreateChangeModel()
		{
			AutorizationModelChange();
		}

		public AccountModel()
		{
		}

		public long Kills { get; set; }
		public long Deaths { get; set;  }
		public long Battles { get; set; }
		public DateTime GameTime { get; set;  }
		public event ChangeModel AutorizationModelChange;
	}
}
