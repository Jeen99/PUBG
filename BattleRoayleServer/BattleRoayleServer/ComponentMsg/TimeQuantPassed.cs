using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleRoayleServer
{
	public class TimeQuantPassed : IComponentMsg
	{
		public GameObject Parent { get; private set; }
		public double QuantTime { get; private set; }
		public TypesComponentMsg Type { get; } = TypesComponentMsg.TimeQuantPassed;

		public TimeQuantPassed(GameObject parent, double quantTime)
		{
			Parent = parent;
			QuantTime = quantTime;
		}
	}
}