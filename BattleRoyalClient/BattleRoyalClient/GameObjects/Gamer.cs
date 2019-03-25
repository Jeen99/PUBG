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
		public override TypesGameObject Type { get; protected set; } = TypesGameObject.Player;

		public Gamer() : base()
		{
		}

		public Gamer(RectangleF shape, double angle = 0) : base(shape, angle)
		{
		}

		public Gamer(PointF location, SizeF size, double angle = 0) : base(location, size, angle)
		{
		}
	}
}
