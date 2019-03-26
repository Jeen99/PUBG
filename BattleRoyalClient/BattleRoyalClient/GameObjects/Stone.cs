using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Common;

namespace BattleRoyalClient
{
	class Stone : IGameObject
	{
		public TypesGameObject Type { get; } = TypesGameObject.Stone;
		private RectangleF shape;
		public RectangleF Shape
		{
			get { return shape; }
			set { shape = value; }
		}
		public PointF Location { get => shape.Location; set => shape.Location = value; }

		public Stone(RectangleF shape)
		{
			this.shape = shape;
		}

		public Stone()
		{
			shape = new RectangleF();
		}
	}
}
