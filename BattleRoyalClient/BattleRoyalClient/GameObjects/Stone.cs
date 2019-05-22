using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.CommonElements;

namespace BattleRoyalClient
{
	class Stone : GameObject
	{
		public Stone(ulong ID) : base(ID)
		{
		}

		public Stone(ulong ID, RectangleF shape, double angle = 0) : base(ID, shape, angle)
		{
		}

		public Stone(ulong ID, PointF location, SizeF size, double angle = 0) : base(ID, location, size, angle)
		{
		}

		public override TypesGameObject Type { get; protected set; } = TypesGameObject.Stone;

		
	}
}
