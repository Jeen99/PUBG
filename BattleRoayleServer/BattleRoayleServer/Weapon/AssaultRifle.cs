using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;
using CommonLibrary.CommonElements;
using System.Drawing;

namespace BattleRoayleServer
{
	class AssaultRifle:Weapon
	{	
		public static SizeF Size { get; protected set; } = new SizeF(18, 5.52f);

		public AssaultRifle(IModelForComponents model, PointF location) : base(model)
		{
			TypeWeapon = TypesWeapon.AssaultRifle;
			physicsSetups = new PhysicsSetups(0, 0, 0.5f, 0.85f);
			weaponSetups = new WeaponSetups(500, 4000, 6);
			size = new SizeF(18, 5.52f);

			var shot = new Shot(this);
			Components.Add(shot);

			base.Setup(location);
		}

		public override TypesBehaveObjects TypesBehave { get; } = TypesBehaveObjects.Active;

		public override TypesGameObject Type { get; } = TypesGameObject.Weapon;
	}
}
