using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Common;
using System.Drawing;

namespace BattleRoayleServer
{
	public class GrenadeCollection:Weapon
	{
		private readonly int TimeBetweenShot = 50;
		private readonly int TimeReload = 10000;
		private readonly int bulletsInMagazin = 4;
		private readonly float strengthThrowGrenade = 30;

		public GrenadeCollection(IModelForComponents model, PointF location) : base(model)
		{
			TypeWeapon = TypesWeapon.GrenadeCollection;

			var body = new TransparentBody(this, new RectangleF(location, new SizeF(8, 8)));
			Components.Add(body);

			var magazin = new Magazin(this, this.TypeWeapon, TimeBetweenShot, TimeReload, bulletsInMagazin);
			Components.Add(magazin);

			var throwGrenade = new Throw(this, strengthThrowGrenade);
			Components.Add(throwGrenade);
		}

		public override TypesBehaveObjects TypesBehave { get; } = TypesBehaveObjects.Passive;

		public override TypesGameObject Type { get; } = TypesGameObject.Weapon;
	}
}
