using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSInteraction.Common;
using System.Drawing;
using System.Collections.Concurrent;

namespace BattleRoayleServer
{
	

	public class Gun : Weapon
	{
		private const float restetution = 0;
		private const float friction = 0.3f;
		private const float density = 0;

		public Gun(PointF location, IGameModel context) : base(context)
		{
			this.Components = new ConcurrentDictionary<Type, Component>();

			SolidBody body = new SolidBody(this, new System.Drawing.RectangleF(location, new SizeF(8,8)),
				restetution, friction, density, TypesBody.Circle, TypesSolid.Transparent, (ushort)CollideCategory.Loot,
				(ushort)CollideCategory.Player);
			Components.AddOrUpdate(body.GetType(), body, (k, v) => { return v; });

			var magazin = new Magazin(this, TypesWeapon.Gun, 500, 3000);
			Components.AddOrUpdate(magazin.GetType(), magazin, (k, v) => { return v; });

			var shot = new Shot(this, magazin);
			Components.AddOrUpdate(shot.GetType(), shot, (k, v) => { return v; });
		}
		public override TypesWeapon TypeWeapon { get; } = TypesWeapon.Gun;

		public override TypesGameObject Type { get; } = TypesGameObject.Weapon;

        public override TypesBehaveObjects TypesBehave { get;  } = TypesBehaveObjects.Passive;

		public override void SetBodyHolder(SolidBody solidBody)
		{
			(GetComponent(typeof(Shot)) as Shot).BodyHolder = solidBody;
		}
	}
}