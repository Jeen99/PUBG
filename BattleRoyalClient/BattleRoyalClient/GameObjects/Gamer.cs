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
		private Bitmap GamerImage;
		private const float Radius = 1.5F;
		public void Draw(Graphics gr)
		{
			if (GamerImage == null)
			{
				GamerImage = new Bitmap(Properties.Recources.Gamer);
			}
			gr.DrawImage(GamerImage, new RectangleF(Location, new Size(30,30)));
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
