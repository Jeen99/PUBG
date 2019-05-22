using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyalClient
{
	interface IAccountModelForView
	{
		long Kills { get; }
		long Deaths { get; }
		long Battles { get; }
		TimeSpan GameTime { get; }
		StatesAccountModel State { get; }
		event ChangeAccountModel AccountModelChange;
	}
}
