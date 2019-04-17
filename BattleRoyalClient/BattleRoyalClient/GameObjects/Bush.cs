using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Common;

namespace BattleRoyalClient
{
	class Bush : GameObject
	{
		public Bush(ulong ID) : base(ID)
		{
		}

		public Bush(ulong ID, RectangleF shape, double angle = 0) : base(ID, shape, angle)
		{
		}

		public Bush(ulong ID, PointF location, SizeF size, double angle = 0) : base(ID, location, size, angle)
		{
		}

		public override TypesGameObject Type { get; protected set; } = TypesGameObject.Bush;
	}
}
