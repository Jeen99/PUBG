using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleRoayleServer
{
	public class TimeQuantPassed : IComponentMsg
	{
		public double QuantTime { get; private set; }
		public TypesComponentMsg Type { get; } = TypesComponentMsg.TimeQuantPassed;

		public TimeQuantPassed(double quantTime = 1.0)
		{
			QuantTime = quantTime;
		}
	}
}