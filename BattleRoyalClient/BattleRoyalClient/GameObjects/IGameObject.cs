using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BattleRoyalClient
{
	interface IGameObject
	{
		void Draw(Graphics gr);
		Tuple<double, double> Location { get; }
	}
}
