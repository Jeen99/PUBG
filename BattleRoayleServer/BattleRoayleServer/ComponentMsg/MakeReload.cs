using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoayleServer
{ 
	class MakeReload : IComponentMsg
	{
		public TypesComponentMsg Type { get; } = TypesComponentMsg.MakeReload;
	}
}
