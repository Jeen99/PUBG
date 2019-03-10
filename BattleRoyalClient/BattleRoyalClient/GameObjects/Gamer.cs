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
		public PointF Location { get; set; }
		private const float Radius = 1.5F;
		public void Draw(Graphics gr)
		{
			using (Brush NewBrush = new SolidBrush(Color.Black))
			{
				gr.FillEllipse(NewBrush, (Location.X - Radius / 2), (Location.Y - Radius / 2),
					(float)Radius, (float)Radius);
			}
		}

		public Gamer(PointF location)
		{
			Location = location;
		}

		public Gamer()
		{
			Location = new PointF(0, 0);
		}
	}
}
