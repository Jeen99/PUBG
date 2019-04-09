using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Common;
using System.Drawing;
using Box2DX.Collision;

namespace BattleRoayleServer
{
	public class ShotGun : Weapon
	{
		public ShotGun(IModelForComponents model, PointF location) : base(model)
		{
			TypeWeapon = TypesWeapon.ShotGun;

			//задаем парамертры модели
			this.TimeBetweenShot = 500;
			this.TimeReload = 5000;
			this.bulletsInMagazin = 3;
			this.size = new SizeF(8, 8);
			this.restetution = 0;
			this.friction = 0;
			this.density = 0.5f;
			this.linearDamping = 0.85f;

			var shot = new Shot(this);
			Components.Add(shot);

			//cоздал этот метод, чтобы сократить повторяющийся код
			base.Setup(location);
		}

		public override TypesBehaveObjects TypesBehave { get; } = TypesBehaveObjects.Active;

		public override TypesGameObject Type { get; } = TypesGameObject.Weapon;
	}
}
