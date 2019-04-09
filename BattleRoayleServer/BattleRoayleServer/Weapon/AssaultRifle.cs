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
	
		public AssaultRifle(IModelForComponents model, PointF location) : base(model)
		{
			TypeWeapon = TypesWeapon.AssaultRifle;

			//задаем парамертры модели
			this.TimeBetweenShot = 500;
			this.TimeReload = 4000;
			this.bulletsInMagazin = 6;
			this.size = new SizeF(8, 8);
			this.restetution = 0;
			this.friction = 0;
			this.density = 0.5f;
			this.linearDamping = 0.85f;

			base.Setup(location);
		}

		public override TypesBehaveObjects TypesBehave { get; } = TypesBehaveObjects.Passive;

		public override TypesGameObject Type { get; } = TypesGameObject.Weapon;
	}
}
