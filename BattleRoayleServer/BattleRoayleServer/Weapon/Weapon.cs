using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Common;
using CSInteraction.ProgramMessage;
using System.Drawing;
using Box2DX.Collision;

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

		public Weapon(IModelForComponents model) : base(model)
		{

		}

		public TypesWeapon TypeWeapon { get; protected set; }

		public IGameObject Holder { get; set; }

		public virtual void Setup(PointF location)
		{
			if (!IsCreate)
			{
				#region CreateShape
				ShapeDef circleShape = new CircleDef();
				(circleShape as CircleDef).Radius = size.Width / 2;
				circleShape.Restitution = restetution;
				circleShape.Friction = friction;
				circleShape.Density = density;
				circleShape.Filter.CategoryBits = (ushort)CollideCategory.Loot;
				circleShape.Filter.MaskBits = (ushort)CollideCategory.Box | (ushort)CollideCategory.Stone;

				ShapeDef sensorDef = new CircleDef();
				(sensorDef as CircleDef).Radius = size.Width / 2;
				sensorDef.Restitution = restetution;
				sensorDef.Friction = friction;
				sensorDef.Density = density;
				sensorDef.IsSensor = true;
				sensorDef.Filter.CategoryBits = (ushort)CollideCategory.Loot;
				sensorDef.Filter.MaskBits = (ushort)CollideCategory.Player;
				#endregion

				var body = new SolidBody(this, new RectangleF(location, size),  new ShapeDef[] { circleShape , sensorDef });
				Components.Add(body);

				var magazin = new Magazin(this, this.TypeWeapon, TimeBetweenShot, TimeReload, bulletsInMagazin);
				Components.Add(magazin);

				var shot = new Shot(this);
				Components.Add(shot);

				IsCreate = true;
			}
		}
	}
}
