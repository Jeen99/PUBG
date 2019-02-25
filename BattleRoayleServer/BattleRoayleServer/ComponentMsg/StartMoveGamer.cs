using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleRoayleServer
{
	public class StartMoveGamer:IComponentMsg
	{
		public Directions Direction { get; private set; }

		public TypesComponentMsg Type { get; private set; }
	}
}