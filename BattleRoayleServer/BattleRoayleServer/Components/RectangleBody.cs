using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleRoayleServer
{
	public class RectangleBody:SolidBody
	{
		public RectangleBody(GameObject owner, IGameModel gameModel, Tuple<double, double> location, Tuple<double, double> size, byte angle, TypesSolid typeSolid) 
			: base(gameModel, owner, location, angle, typeSolid)
		{
			Size = size;
		}

		public Tuple<double, double> Size { get; private set; }
	}
}