using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Common;

namespace BattleRoyalClient
{
	class Gamer : GameObject
	{
		public Gamer()
		{
			base.Type = TypesGameObject.Player;		// ПЕРЕДЕЛАТЬ!!
		}

		public Gamer(RectangleF shape, double angle = 0) : base(shape, angle)
		{
			base.Type = TypesGameObject.Player;
		}

		public Gamer(PointF location, SizeF size, double angle = 0) : base(location, size, angle)
		{
			base.Type = TypesGameObject.Player;
		}
	}
}
