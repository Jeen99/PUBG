using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleRoayleServer
{
	public interface IComponentEvent
	{
		TypesComponentEvent Type { get; set; }
		Tuple<double, double> CentreHappenedEvent { get; }
	}
}