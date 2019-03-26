using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Common;

namespace BattleRoyalClient
{
	class Box : GameObject
	{
		public override TypesGameObject Type { get; protected set; } = TypesGameObject.Box;

		public Box() : base()
		{
		}

		public Box(RectangleF shape, double angle = 0) : base(shape, angle)
		{
		}

		public Box(PointF location, SizeF size, double angle = 0) : base(location, size, angle)
		{
		}
	}
}
