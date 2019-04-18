using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Common;

namespace BattleRoyalClient
{
	class Grenade : GameObject
	{
		public Grenade(ulong ID) : base(ID)
		{

		}

		public Grenade(ulong ID, PointF location, SizeF size, double angle = 0) : base(ID, location, size, angle)
		{
		}

		public override TypesGameObject Type { get; protected set; } = TypesGameObject.Grenade;
	}
}
