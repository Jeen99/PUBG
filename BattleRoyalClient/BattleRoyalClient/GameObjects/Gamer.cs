using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyalClient
{
	class Gamer : IGameObject
	{
		public Tuple<double, double> Location { get; set; }
		private const double Radius = 1.5;
		public void Draw(Graphics gr)
		{
			using (Brush NewBrush = new SolidBrush(Color.Black))
			{
				gr.FillEllipse(NewBrush, (float)(Location.Item1 - Radius / 2), (float)(Location.Item2 - Radius / 2),
					(float)Radius, (float)Radius);
			}
		}

		public Gamer(Tuple<double, double> location)
		{
			Location = location;
		}

		public Gamer()
		{
			Location = new Tuple<double, double>(0,0);
		}
	}
}
