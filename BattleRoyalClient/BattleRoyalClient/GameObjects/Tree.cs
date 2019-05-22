using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.CommonElements;

namespace BattleRoyalClient
{
	class Tree : GameObject
	{
		public Tree(ulong ID) : base(ID)
		{
		}

		public Tree(ulong ID, RectangleF shape, double angle = 0) : base(ID, shape, angle)
		{
		}

		public Tree(ulong ID, PointF location, SizeF size, double angle = 0) : base(ID, location, size, angle)
		{

		}

		public override TypesGameObject Type { get; protected set; } = TypesGameObject.Tree;

		public override void Update(RectangleF rectangle, double Angle = 0)
		{
			Shape = new RectangleF(rectangle.Location, new SizeF(3.3f * rectangle.Width, 3.3f * rectangle.Height));
		}
	}
}
