using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyalClient
{
	interface IAccountModel
	{
		long Kills { get; }
		long Deaths { get; }
		long Battles { get; }
		DateTime GameTime { get; }
		event ChangeModel AutorizationModelChange;
	}
}
