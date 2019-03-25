using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Common;

namespace BattleRoyalClient
{
	class Stone : GameObject
	{
		public Stone()
		{
			base.Type = TypesGameObject.Stone;     // ПЕРЕДЕЛАТЬ!!
		}

		public Stone(RectangleF shape, double angle = 0) : base(shape, angle)
		{
			base.Type = TypesGameObject.Stone;
		}

		public Stone(PointF location, SizeF size, double angle = 0) : base(location, size, angle)
		{
			base.Type = TypesGameObject.Stone;
		}
	}
}
