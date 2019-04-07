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
		private readonly int TimeBetweenShot = 500;
		private readonly int TimeReload = 3000;
		private readonly int bulletsInMagazin = 8;
		private readonly SizeF sizeGun = new SizeF(5, 5);

		public Gun( IModelForComponents model, PointF location) : base(model)
		{
			TypeWeapon = TypesWeapon.Gun;

			var body = new TransparentBody(this, new RectangleF(location, sizeGun));
			Components.Add(body);

			var magazin = new Magazin(this, this.TypeWeapon, TimeBetweenShot, TimeReload, bulletsInMagazin);
			Components.Add(magazin);

			var shot = new Shot(this);
			Components.Add(shot);

			model.AddLoot(this);
		}
		
		public override TypesGameObject Type { get; } = TypesGameObject.Weapon;

        public override TypesBehaveObjects TypesBehave { get;  } = TypesBehaveObjects.Passive;

	}
}