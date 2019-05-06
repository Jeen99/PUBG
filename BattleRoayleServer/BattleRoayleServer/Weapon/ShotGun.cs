using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;
using CommonLibrary.CommonElements;
using System.Drawing;
using Box2DX.Collision;

namespace BattleRoayleServer
{
	public class ShotGun : Weapon
	{
		public static SizeF Size { get; protected set; } = new SizeF(18.66f, 4.2f);

		public ShotGun(IModelForComponents model, PointF location) : base(model)
		{
			TypeWeapon = TypesWeapon.ShotGun;
			physicsSetups = new PhysicsSetups(0, 0, 0.5f, 0.85f);
			weaponSetups = new WeaponSetups(500, 5000, 3);		
			size = new SizeF(18.66f, 4.2f);

			var shot = new Shot(this);
			Components.Add(shot);

			//cоздал этот метод, чтобы сократить повторяющийся код
			base.Setup(location);
		}

		public override TypesBehaveObjects TypesBehave { get; } = TypesBehaveObjects.Active;

		public override TypesGameObject Type { get; } = TypesGameObject.Weapon;
	}
}
