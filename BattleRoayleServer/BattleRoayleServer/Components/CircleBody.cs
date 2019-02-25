using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleRoayleServer
{
	public class CircleBody : SolidBody
	{
		public CircleBody(GameObject owner, IGameModel gameModel, Tuple<double, double> location, double radius, byte angle, TypesSolid typeSolid) 
			: base(gameModel, owner, location, angle, typeSolid)
		{
			Radius = radius;
		}

		public double Radius { get; private set; }
		
	}
}