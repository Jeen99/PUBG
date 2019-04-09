using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoayleServer
{
	interface ICollectorItem
	{
		IGameObject Holder { get; set; }
	}
}
