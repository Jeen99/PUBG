using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleRoayleServer
{
	public class TimeQuantPassed : IComponentMsg
	{
		public TypesComponentMsg Type { get; private set; }
	}
}