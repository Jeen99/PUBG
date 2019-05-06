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

		protected PhysicsSetups physicsSetups;
		protected WeaponSetups weaponSetups;
		protected SizeF size { get;  set; }

		//чтобы не пересоздавать структуры
		private ShapeDef circleShape;
		private ShapeDef sensorDef;

		static Weapon()
		{

		}


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

				var body = new SolidBody(this, new RectangleF(location, size), new ShapeDef[] { circleShape, sensorDef });
				Components.Add(body);

				var magazin = new Magazin(this, this.TypeWeapon, weaponSetups.timeBetweenShot,
					 weaponSetups.timeReload, weaponSetups.bulletsInMagazin);
				Components.Add(magazin);

				IsCreate = true;
			}
		}

		private void CreateBodyShape()
		{
			circleShape = new CircleDef();
			(circleShape as CircleDef).Radius = size.Width / 2;
			circleShape.Restitution = physicsSetups.restetution;
			circleShape.Friction = physicsSetups.friction;
			circleShape.Density = physicsSetups.density;
			circleShape.Filter.CategoryBits = (ushort)CollideCategory.Loot;
			circleShape.Filter.MaskBits = (ushort)CollideCategory.Box | (ushort)CollideCategory.Stone;

			sensorDef = new CircleDef();
			(sensorDef as CircleDef).Radius = size.Width / 2;
			sensorDef.Restitution = physicsSetups.restetution;
			sensorDef.Friction = physicsSetups.friction;
			sensorDef.Density = physicsSetups.density;
			sensorDef.IsSensor = true;
			sensorDef.Filter.CategoryBits = (ushort)CollideCategory.Loot;
			sensorDef.Filter.MaskBits = (ushort)CollideCategory.Player;
		}

		public void CreateNewBody(PointF location, Vec2 startVelocity)
		{
			if (circleShape == null || sensorDef == null) CreateBodyShape();

			var body = new SolidBody(this, new RectangleF(location, size), new ShapeDef[] { circleShape, sensorDef },
				physicsSetups.linearDamping, startVelocity);
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

	public struct WeaponSetups
	{
		public readonly int timeBetweenShot;
		public readonly int timeReload;
		public readonly int bulletsInMagazin;

		public WeaponSetups(int timeBetweenShot, int timeReload, int bulletsInMagazin)
		{
			this.timeBetweenShot = timeBetweenShot;
			this.timeReload = timeReload;
			this.bulletsInMagazin = bulletsInMagazin;
		}
	}
}
