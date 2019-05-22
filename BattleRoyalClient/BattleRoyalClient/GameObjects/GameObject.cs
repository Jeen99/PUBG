using System.Drawing;
using System.Windows.Media.Media3D;
using CommonLibrary.CommonElements;

namespace BattleRoyalClient
{
	abstract class GameObject : IModelObject
	{
		public abstract TypesGameObject Type { get; protected set; }

		private RectangleF shape;
		public RectangleF Shape
		{
			get { return this.shape; }
			set { this.shape = value; }
		}

		public PointF Location
		{
			get
			{
				return Shape.Location;
			}
		}

		public Point3D Location3D
		{
			get
			{
				var x = Shape.X;
				var y = Shape.Y;
				var z = (double)Type;
		
				return new Point3D(x, y, z);
			}
			protected set
			{
				var x = (float)value.X;
				var y = (float)value.Y;
				shape.Location = new PointF(x, y);
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

		public ulong ID { get; protected set; }

		public virtual string TextureName
		{
			get { return Type.ToString(); }
		}

		public GameObject(ulong ID)
		{
			this.ID = ID;
			shape = new RectangleF();
		}

		public GameObject(ulong ID, PointF location, SizeF size, double angle = 0)
		{
			this.ID = ID;
			shape = new RectangleF(location, size);
			this.Angle = angle;
		}

		public GameObject(ulong ID, RectangleF shape, double angle = 0)
		{
			this.ID = ID;
			this.Shape = shape;
			this.Angle = angle;
		}

		public virtual void Update(RectangleF rectangle, double Angle = 0)
		{
			Shape = rectangle;
		}

		public virtual void Update(PointF location, double Angle = 0)
		{
			this.shape.Location = location;	// переделать
		}
		public virtual void Update(double angle)
		{
			this.Angle = angle;
		}
	}
}
