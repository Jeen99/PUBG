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
	public class SolidBody : Component, ISolidBody
	{
		private RectangleF shape;
		//на данный момент временное поле
		public RectangleF Shape { get { return shape; } }

		public Body Body { get; private set; }
		public List<ISolidBody> CoveredObjects { get; } = new List<ISolidBody>();

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

		public SolidBody(IGameObject parent,  RectangleF shape, float restetution, float friction,
			float density, TypesBody typesBody,ushort categoryBits, ushort maskBits): base(parent)
		{
			this.shape = shape;

					switch (typesBody)
					{
						case TypesBody.Circle:
							CreateCircleBody(restetution, friction, density, categoryBits, maskBits);
							break;
						case TypesBody.Rectangle:
							CreateRectangleBody(restetution, friction, density, categoryBits, maskBits);
							break;
					}
			
		}

		private void CreateCircleBody(float restetution, float friction, float density,
			ushort categoryBits, ushort maskBits)
		{
			BodyDef bDef = new BodyDef();
			bDef.Position.Set(shape.X, shape.Y);
			bDef.Angle = 0;
			bDef.FixedRotation = true;

			CircleDef pDef = new CircleDef();
			pDef.Restitution = restetution;
			pDef.Friction = friction;
			pDef.Density = density;
			pDef.Radius = shape.Width / 2;
			pDef.Filter.CategoryBits = categoryBits;
			pDef.Filter.MaskBits = maskBits;

			Body = Parent.Model.Field.CreateBody(bDef);
			Body.CreateShape(pDef);
			Body.SetMassFromShapes();
			Body.SetUserData(this);

		}

		private void CreateRectangleBody(float restetution, float friction, float density,
			ushort categoryBits, ushort maskBits)
		{
			BodyDef bDef = new BodyDef();
			bDef.Position.Set(shape.Left, shape.Top);
			bDef.Angle = 0;

			PolygonDef pDef = new PolygonDef();
			pDef.Restitution = restetution;
			pDef.Friction = friction;
			pDef.Density = density;
			pDef.SetAsBox(shape.Width / 2, shape.Height / 2);
			pDef.Filter.CategoryBits = categoryBits;
			pDef.Filter.MaskBits = maskBits;

			Body = Parent.Model.Field.CreateBody(bDef);
			Body.CreateShape(pDef);
			Body.SetMassFromShapes();
			Body.SetUserData(this);
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
				Parent.Model?.AddEvent(new PlayerMoved(Parent.ID, shape.Location));
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

		
		public void BodyDelete()
		{
			//удаляем объект с карты
			Body.GetWorld().DestroyBody(Body);
			Parent.Model?.AddEvent(new DeleteInMap(Parent.ID));
		}

		public override void Setup()
		{

		}
	}

	public enum CollideCategory
	{
		Player = 0x0001,
		Stone = 0x0002,
		Box = 0x0004
	}

	public enum TypesBody
	{
		Rectangle, 
		Circle
	}
}