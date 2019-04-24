using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using CSInteraction.Common;
using CSInteraction.ProgramMessage;
using Box2DX.Common;
using Box2DX.Collision;
using Box2DX.Dynamics;


namespace BattleRoayleServer
{
	public class SolidBody : Component
	{
		private RectangleF shape;
		//на данный момент временное поле
		public RectangleF Shape { get { return shape; } }

		public Body Body { get; private set; }
		public List<SolidBody> CoveredObjects { get; } = new List<SolidBody>();

		//только для тестов
		public SolidBody(IGameObject parent):base(parent)
		{
			shape = new RectangleF(60,70, 5,5);
			BodyDef bDef = new BodyDef();
			bDef.Position.Set(shape.X, shape.Y);
			bDef.Angle = 0;
			bDef.FixedRotation = true;

			CircleDef pDef = new CircleDef();
			pDef.Restitution = 0;
			pDef.Friction = 0;
			pDef.Density = 0.5f;
			pDef.Radius = shape.Width / 2;
			pDef.Filter.CategoryBits = 0;
			pDef.Filter.MaskBits = 0;

			Body = Parent.Model.Field.CreateBody(bDef);
			Body.CreateShape(pDef);
			Body.SetMassFromShapes();
			Body.SetUserData(this);
		}

		public SolidBody(IGameObject parent, RectangleF shape, ShapeDef[] shapesForBox2D) : base(parent)
		{
			if(shapesForBox2D == null || shapesForBox2D.Length == 0)
				throw new Exception("Невозможно создать тело без описания хотя бы одной его формы");

			this.shape = shape;

			BodyDef bDef = new BodyDef();
			bDef.Position.Set(shape.X, shape.Y);
			bDef.Angle = 0;
			bDef.FixedRotation = true;

			Body = Parent.Model.Field.CreateBody(bDef);
			Body.SetUserData(this);


			for (int i = 0; i < shapesForBox2D.Length; i++)
			{
				Body.CreateShape(shapesForBox2D[i]);
			}

			Body.SetMassFromShapes();
		}

		public SolidBody(IGameObject parent, RectangleF shape, ShapeDef[] shapesForBox2D, 
			float linerDamping, Vec2 startVelocity) : base(parent)
		{
			if (shapesForBox2D == null || shapesForBox2D.Length == 0)
				throw new Exception("Невозможно создать тело без описания хотя бы одной его формы");

			this.shape = shape;

			BodyDef bDef = new BodyDef();
			bDef.Position.Set(shape.X, shape.Y);
			bDef.Angle = 0;
			bDef.LinearDamping = linerDamping;
			bDef.FixedRotation = true;

			Body = Parent.Model.Field.CreateBody(bDef);
			Body.SetLinearVelocity(startVelocity);
			Body.SetUserData(this);


			for (int i = 0; i < shapesForBox2D.Length; i++)
			{
				Body.CreateShape(shapesForBox2D[i]);
			}
					
			Body.SetMassFromShapes();			
		}
	
		public override void UpdateComponent(IMessage msg)
		{
			if (msg == null)
			{
				Log.AddNewRecord("Получено null сообщение в компоненте SolidBody");
				return;
			}

			switch (msg.TypeMessage)
			{
				case TypesProgramMessage.TimeQuantPassed:
					Handler_TimeQuantPassed(msg as TimeQuantPassed);					
					break;
			}
			
		}

		private void Handler_TimeQuantPassed(TimeQuantPassed msg)
		{
			Vec2 position = Body.GetPosition();
			if (position.X != shape.X || position.Y != shape.Y)
			{
				shape.Location = new PointF(position.X, position.Y);
				Parent.Model?.AddEvent(new ObjectMoved(Parent.ID, shape.Location));
			}
		}
		
		public override void Dispose()
		{
			CoveredObjects.Clear();
			//удаляем объект с карты
			Body.GetWorld().DestroyBody(Body);
		}
		public override IMessage State
		{
			get
			{
				return new BodyState(shape);
			}
		}

		public override void Setup()
		{

		}
	}

	public enum CollideCategory
	{
		Player = 0x0001,
		Stone = 0x0002,
		Box = 0x0004,
		Grenade = 0x0008,
		Loot = 0x0016
	}

	public enum TypesBody
	{
		Rectangle, 
		Circle
	}

	
}