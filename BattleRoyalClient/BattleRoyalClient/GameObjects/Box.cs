using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Common;

namespace BattleRoyalClient
{
	class Box : IGameObject
	{
		public TypesGameObject Type{ get; } = TypesGameObject.Box;

		private RectangleF shape;
		public RectangleF Shape {
			get { return shape; }
			set { shape = value; }
		}
		public PointF Location { get => shape.Location; set => shape.Location = value; }

		public Box(RectangleF shape)
		{
			this.shape = shape;
		}

		public Box()
		{
			shape = new RectangleF();
		}
	}
}
