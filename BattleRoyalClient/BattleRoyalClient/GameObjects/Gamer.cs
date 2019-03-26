using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Common;

namespace BattleRoyalClient
{
	class Gamer : IGameObject
	{
		private RectangleF shape;
		public RectangleF Shape
		{
			get { return shape; }
			set { shape = value; }
		}
		public PointF Location { get => shape.Location; set => shape.Location = value; }
		public PointF OldLocation { get; set; }
		public TypesGameObject Type { get; } = TypesGameObject.Player;

		public Gamer(RectangleF shape) 
		{
			this.shape = shape;
		}

		public Gamer()
		{
			shape = new RectangleF();
		}
	}
}
