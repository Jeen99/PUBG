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
		public Box()
		{
			base.Type = TypesGameObject.Box;     // ПЕРЕДЕЛАТЬ!!
		}

		public Box(RectangleF shape, double angle = 0) : base(shape, angle)
		{
			base.Type = TypesGameObject.Box;     // ПЕРЕДЕЛАТЬ!!
		}

		public Box(PointF location, SizeF size, double angle = 0) : base(location, size, angle)
		{
			base.Type = TypesGameObject.Box;     // ПЕРЕДЕЛАТЬ!!
		}
	}
}
