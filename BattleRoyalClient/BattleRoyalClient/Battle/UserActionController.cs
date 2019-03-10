using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Client;

namespace BattleRoyalClient
{
	class UserActionController
	{
		private BaseClient client;

		public UserActionController(BaseClient client)
		{
			this.client = client;
		}
	}
}
