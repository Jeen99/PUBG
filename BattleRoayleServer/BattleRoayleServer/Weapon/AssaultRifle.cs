using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Common;
using System.Drawing;

namespace BattleRoayleServer
{
	class AssaultRifle:Weapon
	{
		private readonly int TimeBetweenShot = 500;
		private readonly int TimeReload = 4000;
		private readonly int bulletsInMagazin = 6;
		private readonly SizeF sizeAssaultRifle = new SizeF(8, 8);

		public AssaultRifle(IModelForComponents model, PointF location) : base(model)
		{
			TypeWeapon = TypesWeapon.AssaultRifle;

			var body = new TransparentBody(this, new RectangleF(location, sizeAssaultRifle));
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
