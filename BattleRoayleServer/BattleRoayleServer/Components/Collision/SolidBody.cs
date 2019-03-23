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
	

		public SolidBody(GameObject parent,  RectangleF shape, float restetution, float friction,
			float density, TypesBody typesBody, TypesSolid typesSolid,
			ushort categoryBits, ushort maskBits)
			: base(parent)
		{
			TypeSolid = typesSolid;
			this.shape = shape;
			switch (TypeSolid)
			{
				case TypesSolid.Solid:
					switch (typesBody)
					{
						case TypesBody.Circle:
							CreateCircleBody(restetution, friction, density, categoryBits, maskBits);
							break;
						case TypesBody.Rectangle:
							CreateRectangleBody(restetution, friction, density, categoryBits, maskBits);
							break;
					}
					break;
				case TypesSolid.Transparent:
					switch (typesBody)
					{
						case TypesBody.Circle:
							CreateTransparentCircleBody(restetution, friction, density, categoryBits, maskBits);
							break;
						case TypesBody.Rectangle:
							CreateTransparentRectangleBody(restetution, friction, density, categoryBits, maskBits);
							break;
					}
					break;
			}

		}
		private void CreateTransparentCircleBody(float restetution, float friction, float density,
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
			pDef.IsSensor = true;
			pDef.Filter.CategoryBits = categoryBits;
			pDef.Filter.MaskBits = maskBits;

			Body = Parent.Model.Field.CreateBody(bDef);
			Body.CreateShape(pDef);
			Body.SetMassFromShapes();
			Body.SetUserData(this);
		}

		private void CreateTransparentRectangleBody(float restetution, float friction, float density,
			ushort categoryBits, ushort maskBits)
		{
			BodyDef bDef = new BodyDef();
			bDef.Position.Set(shape.Right, shape.Bottom);
			bDef.Angle = 0;

			PolygonDef pDef = new PolygonDef();
			pDef.Restitution = restetution;
			pDef.Friction = friction;
			pDef.Density = density;
			pDef.SetAsBox(shape.Width / 2, shape.Height / 2);
			pDef.Filter.CategoryBits = categoryBits;
			pDef.Filter.MaskBits = maskBits;
			pDef.IsSensor = true;

			Body = Parent.Model.Field.CreateBody(bDef);
			Body.CreateShape(pDef);
			Body.SetMassFromShapes();
			Body.SetUserData(this);
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
			bDef.Position.Set(shape.Right, shape.Bottom);
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

		public TypesSolid TypeSolid { get; private set; }

		public override void ProcessMsg(IComponentMsg msg)
		{
			if (msg != null)
			{
				switch (msg.Type)
				{
					case TypesComponentMsg.TimeQuantPassed:
						Handler_TimeQuantPassed();
						break;
				}
			}

		}
		private void Handler_TimeQuantPassed()
		{

		}
		public void BodyMove()
		{
			Vec2 position = Body.GetPosition();
			shape.Location = new PointF(position.X, position.Y);
			Parent.Model.HappenedEvents.Enqueue(new PlayerMoved(Parent.ID, shape.Location));
		}
		public void SendMessage(IComponentMsg msg)
		{
			Parent.SendMessage(msg);
		}

		public override void Dispose()
		{
			throw new NotImplementedException();
		}
		public override IMessage State
		{
			get
			{
				return new BodyState(shape);
			}
		}

		//возвращает коллекцию объектов, которые можно поднять
		public List<SolidBody> GetPickUpObjects()
		{
			List<SolidBody> pickUpObjects = new List<SolidBody>();
			  
			foreach (var gameObject in CoveredObjects)
			{
				switch (gameObject.Parent.Type)
				{
					case TypesGameObject.Gun:
						pickUpObjects.Add(gameObject);
						break;
				}
			}
			return pickUpObjects;
		}

		public void BodyDelete()
		{
			Parent.Model.NeedDelete.Add(this);
			Parent.Model.HappenedEvents.Enqueue(new GameObjectDelete(this.Parent.ID));
		}
	}

	public enum CollideCategory
	{
		Player = 0x0001,
		Loot = 0x0002,
		Box = 0x0003,
		Stone = 0x0004
	}

	public enum TypesSolid
	{
		Solid,
		Transparent
	}

	public enum TypesBody
	{
		Rectangle, 
		Circle
	}
}