using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLibrary;
using CommonLibrary.CommonElements;
using System.Drawing;
using System.Collections.Concurrent;

namespace BattleRoayleServer
{	
	public class Gun : Weapon
	{
		
		public static SizeF Size { get; protected set; } = new SizeF(8.6f, 5.6f);

		public Gun( IModelForComponents model, PointF location): base(model)
		{
			TypeWeapon = TypesWeapon.Gun;
			physicsSetups = new PhysicsSetups(0, 0, 0.5f, 0.85f);
			weaponSetups = new WeaponSetups(500, 3000, 8);
			size = new SizeF(8.6f, 5.6f);

			var shot = new Shot(this);
			Components.Add(shot);

			base.Setup(location);

		}
		
		public override TypesGameObject Type { get; } = TypesGameObject.Weapon;

        public override TypesBehaveObjects TypesBehave { get;  } = TypesBehaveObjects.Active;

	}
}