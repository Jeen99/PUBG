using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;
using CommonLibrary.CommonElements;
using System.Drawing;
using Box2DX.Collision;
using Box2DX.Common;
using CommonLibrary.GameMessages;

namespace BattleRoayleServer
{
	public abstract class Weapon : GameObject, IWeapon, ICollectorItem
	{
		private bool IsCreate = false;

		protected float restetution;
		protected float friction;
		protected float density;
		protected float linearDamping;
		protected SizeF size;

		protected int TimeBetweenShot;
		protected int TimeReload;
		protected int bulletsInMagazin;

		//чтобы не пересоздавать структуры
		private ShapeDef circleShape;
		private ShapeDef sensorDef;

		public Weapon(IModelForComponents model) : base(model)
		{

		}

		public TypesWeapon TypeWeapon { get; protected set; }

		public IGameObject Holder { get; set; }

		public virtual void Setup(PointF location)
		{
			if (!IsCreate)
			{
				CreateBodyShape();

				var body = new SolidBody(this, new RectangleF(location, size),  new ShapeDef[] { circleShape , sensorDef });
				Components.Add(body);

				var magazin = new Magazin(this, this.TypeWeapon, TimeBetweenShot, TimeReload, bulletsInMagazin);
				Components.Add(magazin);

				IsCreate = true;
			}
		}

		private void CreateBodyShape()
		{
			circleShape = new CircleDef();
			(circleShape as CircleDef).Radius = size.Width / 2;
			circleShape.Restitution = restetution;
			circleShape.Friction = friction;
			circleShape.Density = density;
			circleShape.Filter.CategoryBits = (ushort)CollideCategory.Loot;
			circleShape.Filter.MaskBits = (ushort)CollideCategory.Box | (ushort)CollideCategory.Stone;

			sensorDef = new CircleDef();
			(sensorDef as CircleDef).Radius = size.Width / 2;
			sensorDef.Restitution = restetution;
			sensorDef.Friction = friction;
			sensorDef.Density = density;
			sensorDef.IsSensor = true;
			sensorDef.Filter.CategoryBits = (ushort)CollideCategory.Loot;
			sensorDef.Filter.MaskBits = (ushort)CollideCategory.Player;
		}

		public void CreateNewBody(PointF location, Vec2 startVelocity)
		{
			if (circleShape == null || sensorDef == null) CreateBodyShape();

			var body = new SolidBody(this, new RectangleF(location, size), new ShapeDef[] { circleShape, sensorDef }, 
				linearDamping, startVelocity);
			Components.Add(body);
		}

		public override IMessage State
		{
			get
			{
				var states = new List<IMessage>();
				if (Destroyed) return null;
				else
				{
					foreach (IComponent component in Components)
					{
						var state = component.State;
						if (state != null)
						{
							states.Add(state);
						}
					}
					return new WeaponState(ID, Type, TypeWeapon, states);
				}
			}
		}
	}
}
