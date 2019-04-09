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
		
		public Gun( IModelForComponents model, PointF location): base(model)
		{
			TypeWeapon = TypesWeapon.Gun;

			//задаем парамертры модели
			this.TimeBetweenShot = 500;
			this.TimeReload = 3000;
			this.bulletsInMagazin = 8;
			this.size = new SizeF(8, 8);
			this.restetution = 0;
			this.friction = 0;
			this.density = 0.5f;
			this.linearDamping = 0.85f;

			var shot = new Shot(this);
			Components.Add(shot);

			base.Setup(location);

		}
		
		public override TypesGameObject Type { get; } = TypesGameObject.Weapon;

        public override TypesBehaveObjects TypesBehave { get;  } = TypesBehaveObjects.Active;

	}
}