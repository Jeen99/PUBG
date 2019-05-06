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
	public class GrenadeCollection:Weapon
	{	
		protected static float strengthThrowGrenade = 100;

		public static SizeF Size { get; protected set; } = new SizeF(5.46f, 8.12f);

		public GrenadeCollection(IModelForComponents model, PointF location) : base(model)
		{
			TypeWeapon = TypesWeapon.GrenadeCollection;
			physicsSetups = new PhysicsSetups(0, 0, 0.5f, 0.85f);
			weaponSetups = new WeaponSetups(500, 10000, 4);		
			size = new SizeF(5.46f, 8.12f);

			var throwGrenade = new Throw(this, strengthThrowGrenade);
			Components.Add(throwGrenade);

			base.Setup(location);
		}

		public override TypesBehaveObjects TypesBehave { get; } = TypesBehaveObjects.Active;

		public override TypesGameObject Type { get; } = TypesGameObject.Weapon;
	}
}
