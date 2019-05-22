using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyalClient
{
	static class StorageModels
	{
		public readonly static AuthorizationModel AutorizationModel = new AuthorizationModel();
		public readonly static AccountModel AccountModel = new AccountModel();
		public readonly static QueueModel QueueModel = new QueueModel();
		public readonly static BattleModel BattleModel = new BattleModel();
	}
}
