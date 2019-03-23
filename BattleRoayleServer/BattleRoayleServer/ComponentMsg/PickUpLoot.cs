using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoayleServer
{
	class PickUpLoot : IComponentMsg
	{
		public TypesComponentMsg Type { get; } = TypesComponentMsg.PickUpLoot;
	}
}
