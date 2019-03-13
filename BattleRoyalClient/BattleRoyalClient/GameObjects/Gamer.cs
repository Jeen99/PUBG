using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Common;

namespace BattleRoyalClient
{
	class Gamer : IGameObject
	{
		public PointF Location { get; set; }
		private Bitmap GamerImage;
		private const float Radius = 1.5F;
		

		public TypesGameObject Type { get; } = TypesGameObject.Player;

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
