using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Common;
using System.Drawing;

namespace BattleRoayleServer
{
	public class ShotGun : Weapon
	{
		private readonly int TimeBetweenShot = 800;
		private readonly int TimeReload = 5000;
		private readonly int bulletsInMagazin = 3;

		public ShotGun(IModelForComponents model, PointF location) : base(model)
		{
			TypeWeapon = TypesWeapon.ShotGun;

			var body = new TransparentBody(this, new RectangleF(location, new SizeF(8, 8)));
			Components.Add(body);

			var magazin = new Magazin(this, this.TypeWeapon, TimeBetweenShot, TimeReload, bulletsInMagazin);
			Components.Add(magazin);

			var shot = new Shot(this);
			Components.Add(shot);

			model.AddLoot(this);
		}

		public override TypesBehaveObjects TypesBehave { get; } = TypesBehaveObjects.Passive;

		public override TypesGameObject Type { get; } = TypesGameObject.Weapon;
	}
}
