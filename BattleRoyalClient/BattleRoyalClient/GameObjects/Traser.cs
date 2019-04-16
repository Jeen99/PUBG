using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Common;
using System.Timers;
using System.Drawing;

namespace BattleRoyalClient
{
	class Traser : GameObject
	{
		
		public Traser(ulong ID) : base(ID)
		{
		}

		public Traser(ulong ID, RectangleF shape, double angle = 0) : base(ID, shape, angle)
		{
		}

		public Traser(ulong ID, PointF location, SizeF size, double angle = 0) : base(ID, location, size, angle)
		{
		}

		public override string TextureName
		{
			get
			{
				return "Traser";
			}
		}

		public override TypesGameObject Type { get; protected set; } = TypesGameObject.Indefinitely;
	}

}
