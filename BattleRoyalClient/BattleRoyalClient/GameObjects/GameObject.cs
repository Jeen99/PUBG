using System.Drawing;
using CSInteraction.Common;

namespace BattleRoyalClient
{
	abstract class GameObject
	{
		public TypesGameObject Type { get; protected set; }

		public Model3D Model3D { get; set; } // вынести из класса

		private RectangleF shape;
		public RectangleF Shape
		{
			get { return this.shape; }
			set { this.shape = value; }
		}

		public PointF Location
		{
			get { return Shape.Location; }
			protected set
			{
				//var rect = Shape;
				//rect.Location = Location;
				shape.Location = value;
			}
		}
		public SizeF Size
		{
			get { return Shape.Size; }
			protected set
			{
				shape.Size = value;
			}
		}

		public double Angle { get; protected set; }

		public GameObject()
		{
			shape = new RectangleF();
		}

		public GameObject(PointF location, SizeF size, double angle = 0)
		{
			shape = new RectangleF(location, size);
			this.Angle = angle;
		}

		public GameObject(RectangleF shape, double angle = 0)
		{
			this.Shape = shape;
			this.Angle = angle;
		}

		public void Update(RectangleF rectangle, double Angle = 0)
		{
			Shape = rectangle;
		}

		public void Update(PointF location, double Angle = 0)
		{
			this.shape.Location = location;	// переделать
		}
	}
}
