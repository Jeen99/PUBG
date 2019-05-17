using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.CommonElements;
using Box2DX.Common;
using Box2DX.Collision;
using Box2DX.Dynamics;
using System.Drawing;

namespace BattleRoyalServer
{
	public static class BuilderGameObject
	{
		public static SizeF SizeBox { get; } = new SizeF(15, 15);
		public static SizeF SizeBush { get; } = new SizeF(15, 15);
		public static SizeF SizeStone { get; } = new Size(16, 16);
		public static SizeF SizeGreanade { get; } = new SizeF(5, 5);
		public static SizeF SizeTree { get; } = new SizeF(15, 15);
		public static SizeF SizeGun { get; } = new SizeF(8.6f, 5.6f);
		public static SizeF SizeAssaulRiffle { get; } = new SizeF(18, 5.52f);
		public static SizeF SizeShotGun { get; } = new SizeF(18.66f, 4.2f);
		public static SizeF SizeGrenadeCollection { get; } = new SizeF(5.46f, 8.12f);
		public static SizeF SizeGamer { get; } = new SizeF(10, 10);

		private static PhysicsSetups setupsStillObject = new PhysicsSetups(0, 0, 0, 0);
		private static PhysicsSetups setupsStoneAndTree = new PhysicsSetups(0, 0.1f, 0, 0);
		private static PhysicsSetups setupsWeapon = new PhysicsSetups(0, 0, 0.5f, 0.85f);
		private static PhysicsSetups setupsGamer = new PhysicsSetups(0, 0, 0.5f, 0);

		private static WeaponSetups weaponSetupsGun = new WeaponSetups(500, 3000, 8);
		private static WeaponSetups weaponSetupsAssaultRiffle = new WeaponSetups(500, 4000, 6);
		private static WeaponSetups weaponSetupsShotGun = new WeaponSetups(500, 5000, 3);
		private static WeaponSetups weaponSetupsGrenadeCollection = new WeaponSetups(500, 10000, 4);
		
		private static float strengthThrowGrenade = 100;
		private static float SpeedGamer = 40f;
		private static float RadiusExplosionGrenade = 20;

		public static GameObject CreateBox(IModelForComponents model, PointF location)
		{

			var gameObject = new GameObject(model, TypesGameObject.Box, TypesBehaveObjects.Passive);

			var shapeBox = CreateBaseRectangleDef(setupsStillObject, SizeBox);
			
			shapeBox.Filter.CategoryBits = (ushort)CollideCategory.Box;
			shapeBox.Filter.MaskBits = (ushort)CollideCategory.Player |
				(ushort)CollideCategory.Grenade | (ushort)CollideCategory.Loot;

			var body = new SolidBody(gameObject, new RectangleF(location, SizeBox), new ShapeDef[] { shapeBox });
			gameObject.Components.Add(body);

			model.AddOrUpdateGameObject(gameObject);

			return gameObject;
		}

		public static GameObject CreateBush(IModelForComponents model, PointF location)
		{
			var gameObject = new GameObject(model,  TypesGameObject.Bush, TypesBehaveObjects.Passive);

			TransparentBody bushBody = new TransparentBody(gameObject, new RectangleF(location, SizeBush));
			gameObject.Components.Add(bushBody);

			model.AddOrUpdateGameObject(gameObject);

			return gameObject;
		}

		public static GameObject CreateDeathZone(IModelForComponents model, float diameter)
		{
			var gameObject = new GameObject(model, TypesGameObject.DeathZone, TypesBehaveObjects.Active);

			BodyZone bodyZone = new BodyZone(gameObject, diameter);
			gameObject.Components.Add(bodyZone);

			DamageZone damageZone = new DamageZone(gameObject);
			gameObject.Components.Add(damageZone);

			model.AddOrUpdateGameObject(gameObject);

			return gameObject;  
		}

		public static GameObject CreateGrenade(IModelForComponents model, PointF location, Vec2 startVelocity, IBullet grenadeBullet)
		{
			var gameObject = new GameObject(model, TypesGameObject.Grenade, TypesBehaveObjects.Active);
			ShapeDef circleShape = CreateBaseCircleDef(setupsWeapon, SizeGreanade);
			circleShape.Filter.CategoryBits = (ushort)CollideCategory.Grenade;
			circleShape.Filter.MaskBits = (ushort)CollideCategory.Box | (ushort)CollideCategory.Stone;

			ShapeDef sensorDef = CreateBaseCircleDef(setupsWeapon, new SizeF(RadiusExplosionGrenade, RadiusExplosionGrenade));
			sensorDef.IsSensor = true;
			sensorDef.Filter.CategoryBits = (ushort)CollideCategory.Grenade;
			sensorDef.Filter.MaskBits = (ushort)CollideCategory.Player;

			var body = new SolidBody(gameObject, new RectangleF(location, SizeGreanade),
				new ShapeDef[] { circleShape, sensorDef }, setupsWeapon.linearDamping, startVelocity);
			gameObject.Components.Add(body);

			var explosion = new Explosion(gameObject, grenadeBullet);
			gameObject.Components.Add(explosion);

			model.AddOrUpdateGameObject(gameObject);

			return gameObject;
		}

		public static GameObject CreateStone(IModelForComponents model, PointF location)
		{
			var gameObject = new GameObject(model, TypesGameObject.Stone, TypesBehaveObjects.Passive);
			ShapeDef CircleDef = CreateBaseCircleDef(setupsStoneAndTree, SizeStone);
			CircleDef.Filter.CategoryBits = (ushort)CollideCategory.Stone;
			CircleDef.Filter.MaskBits = (ushort)CollideCategory.Player |
				(ushort)CollideCategory.Grenade | (ushort)CollideCategory.Loot;

			var body = new SolidBody(gameObject, new RectangleF(location, SizeStone), new ShapeDef[] { CircleDef });
			gameObject.Components.Add(body);

			model.AddOrUpdateGameObject(gameObject);

			return gameObject;
		}

		public static GameObject CreateTree(IModelForComponents model, PointF location)
		{
			var gameObject = new GameObject(model, TypesGameObject.Tree, TypesBehaveObjects.Passive);

			ShapeDef CircleDef = CreateBaseCircleDef(setupsStoneAndTree, SizeTree);
			CircleDef.Filter.CategoryBits = (ushort)CollideCategory.Stone;
			CircleDef.Filter.MaskBits = (ushort)CollideCategory.Player |
				(ushort)CollideCategory.Grenade | (ushort)CollideCategory.Loot;

			var body = new SolidBody(gameObject, new RectangleF(location, SizeStone), new ShapeDef[] { CircleDef });
			gameObject.Components.Add(body);

			model.AddOrUpdateGameObject(gameObject);

			return gameObject;
		}

		public static GameObject CreateGun(IModelForComponents model, PointF location)
		{
			GameObject gameObject = CreateShotWeapon(model, setupsWeapon, weaponSetupsGun, SizeGun, TypesWeapon.Gun, location);
			model.AddOrUpdateGameObject(gameObject);
			return gameObject;
		}

		public static GameObject CreateAssaultRiffle(IModelForComponents model, PointF location)
		{
			GameObject gameObject = CreateShotWeapon(model, setupsWeapon, weaponSetupsAssaultRiffle, SizeAssaulRiffle, TypesWeapon.AssaultRifle, location);
			model.AddOrUpdateGameObject(gameObject);
			return gameObject;
		}

		public static GameObject CreateShotGun(IModelForComponents model, PointF location)
		{
			GameObject gameObject = CreateShotWeapon(model, setupsWeapon, weaponSetupsShotGun, SizeShotGun, TypesWeapon.ShotGun, location);
			model.AddOrUpdateGameObject(gameObject);
			return gameObject;
		}

		public static GameObject CreateGrenadeCollection(IModelForComponents model, PointF location)
		{
			var gameObject = new Weapon(model, TypesGameObject.Weapon, TypesBehaveObjects.Active, TypesWeapon.GrenadeCollection);
			CreateStandartComponentForWeapon(gameObject, setupsWeapon, weaponSetupsGrenadeCollection,
				SizeGrenadeCollection, TypesWeapon.GrenadeCollection, location);

			var throwGrenade = new Throw(gameObject, strengthThrowGrenade);
			gameObject.Components.Add(throwGrenade);

			model.AddOrUpdateGameObject(gameObject);

			return gameObject;
		}

		public static Gamer CreateGamer(IModelForComponents model, PointF location)
		{
			var gameObject = new Gamer(model, TypesBehaveObjects.Active, true);
			ShapeDef CircleDef = CreateBaseCircleDef(setupsGamer, SizeGamer);
			CircleDef.Filter.CategoryBits = (ushort)CollideCategory.Player;
			CircleDef.Filter.MaskBits = (ushort)CollideCategory.Box | (ushort)CollideCategory.Stone
				| (ushort)CollideCategory.Grenade;

			var body = new SolidBody(gameObject, new RectangleF(location, SizeGamer), new ShapeDef[] { CircleDef });
			gameObject.Components.Add(body);

			var movement = new Movement(gameObject, SpeedGamer);
			gameObject.Components.Add(movement);

			var collector = new Collector(gameObject);
			gameObject.Components.Add(collector);

			var currentWeapon = new CurrentWeapon(gameObject);
			gameObject.Components.Add(currentWeapon);

			var healthy = new Healthy(gameObject);
			gameObject.Components.Add(healthy);

			var statistics = new Statistics(gameObject);
			gameObject.Components.Add(statistics);

			model.AddOrUpdateGameObject(gameObject);
			model.Players.Add(gameObject);

			return gameObject;
		}

		public static GameObject CreateBot(IModelForComponents model, PointF location)
		{
			var gameObject = new Gamer(model, TypesBehaveObjects.Active, false);

			ShapeDef CircleDef = CreateBaseCircleDef(setupsGamer, SizeGamer);
			CircleDef.Filter.CategoryBits = (ushort)CollideCategory.Player;
			CircleDef.Filter.MaskBits = (ushort)CollideCategory.Box | (ushort)CollideCategory.Stone
				| (ushort)CollideCategory.Grenade;

			var body = new SolidBody(gameObject, new RectangleF(location, SizeGamer), new ShapeDef[] { CircleDef });
			gameObject.Components.Add(body);

			var movement = new Movement(gameObject, SpeedGamer);
			gameObject.Components.Add(movement);

			var collector = new Collector(gameObject);
			gameObject.Components.Add(collector);

			var currentWeapon = new CurrentWeapon(gameObject);
			gameObject.Components.Add(currentWeapon);

			var healthy = new Healthy(gameObject);
			gameObject.Components.Add(healthy);

			var AI = new AI(gameObject);
			gameObject.Components.Add(AI);

			model.AddOrUpdateGameObject(gameObject);
			model.Players.Add(gameObject);
			#warning Возможно, нет необходимости добавлять в список Игроков

			return gameObject;
		}

		public static void CreateNewBodyForWeapon(Weapon weapon, Vec2 startVelocity, PointF location)
		{
			SolidBody body = null;
			switch (weapon.TypeWeapon)
			{
				case TypesWeapon.AssaultRifle:
					body = CreateBodyWeapon(weapon,location, setupsWeapon, SizeAssaulRiffle);
					break;
				case TypesWeapon.GrenadeCollection:
					body = CreateBodyWeapon(weapon, location, setupsWeapon, SizeGrenadeCollection);
					break;
				case TypesWeapon.Gun:
					body = CreateBodyWeapon(weapon, location, setupsWeapon, SizeGun);
					break;
				case TypesWeapon.ShotGun:
					body = CreateBodyWeapon(weapon, location, setupsWeapon, SizeShotGun);
					break;
				default: throw new Exception("Ошибка создания тела для оружия");
			}
			body.Body.SetLinearVelocity(startVelocity);
			weapon.Components.Add(body);

			weapon.Model.AddOrUpdateGameObject(weapon);
		}

		private static Weapon CreateShotWeapon(IModelForComponents model, PhysicsSetups physicsSetupsWeapon, 
			WeaponSetups weaponSetups, SizeF sizeWeapon, TypesWeapon typeWeapon, PointF location)
		{
			var weapon = new Weapon(model, TypesGameObject.Weapon, TypesBehaveObjects.Active, typeWeapon);
			CreateStandartComponentForWeapon(weapon, physicsSetupsWeapon, weaponSetups, sizeWeapon, typeWeapon, location);

			var shot = new Shot(weapon);
			weapon.Components.Add(shot);

			return weapon;
		}

		private static void CreateStandartComponentForWeapon(Weapon weapon, PhysicsSetups physicsSetupsWeapon,
			WeaponSetups weaponSetups, SizeF sizeWeapon, TypesWeapon typeWeapon, PointF location)
		{
			CreateBodyWeapon(weapon, location, physicsSetupsWeapon, sizeWeapon);
			CreateMagazin(weapon, weaponSetups, typeWeapon);
		}

		private static SolidBody CreateBodyWeapon(Weapon weapon, PointF location, PhysicsSetups physicsSetupsWeapon , SizeF sizeWeapon)
		{
			ShapeDef circleShape = CreateBaseCircleDef(physicsSetupsWeapon, sizeWeapon);
			circleShape.Filter.CategoryBits = (ushort)CollideCategory.Loot;
			circleShape.Filter.MaskBits = (ushort)CollideCategory.Box | (ushort)CollideCategory.Stone;

			ShapeDef sensorDef = CreateBaseCircleDef(physicsSetupsWeapon, sizeWeapon);
			sensorDef.IsSensor = true;
			sensorDef.Filter.CategoryBits = (ushort)CollideCategory.Loot;
			sensorDef.Filter.MaskBits = (ushort)CollideCategory.Player;

			var body = new SolidBody(weapon, new RectangleF(location, sizeWeapon), new ShapeDef[] { circleShape, sensorDef },
				physicsSetupsWeapon.linearDamping, Vec2.Zero);
			weapon.Components.Add(body);

			return body;
		}

		private static Magazin CreateMagazin(Weapon weapon, WeaponSetups weaponSetup, TypesWeapon typeWeapon)
		{
			var magazin = new Magazin(weapon, typeWeapon, weaponSetup.timeBetweenShot,
				weaponSetup.timeReload, weaponSetup.bulletsInMagazin);
			weapon.Components.Add(magazin);

			return magazin;
		}

		private static ShapeDef CreateBaseRectangleDef(PhysicsSetups setups, SizeF size)
		{
			ShapeDef RectangleDef = new PolygonDef();
			(RectangleDef as PolygonDef).SetAsBox(size.Width / 2, size.Height / 2);
			RectangleDef.Restitution = setups.restetution;
			RectangleDef.Friction = setups.friction;
			RectangleDef.Density = setups.density;

			return RectangleDef;
		}

		private static ShapeDef CreateBaseCircleDef(PhysicsSetups setups, SizeF size)
		{
			ShapeDef circleShape = new CircleDef();
			(circleShape as CircleDef).Radius = size.Width / 2;
			circleShape.Restitution = setups.restetution;
			circleShape.Friction = setups.friction;
			circleShape.Density = setups.density;

			return circleShape;
		}

	}
}
