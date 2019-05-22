using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleRoyalServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerTest.Common;
using System.Drawing;
using CommonLibrary.CommonElements;

namespace BattleRoyalServer.Tests
{
	[TestClass()]
	public class BuilderGameObjectTests
	{
		[TestMethod()]
		public void CreateBoxTest()
		{
			var location = new PointF(10, 10);
			var model = new RoyalGameModel();
			var obj = BuilderGameObject.CreateBox(model, location);

			var solidBody = obj.Components.GetComponent<SolidBody>();
			Assert.IsNotNull(solidBody);

			Assert.AreEqual(location, solidBody.Shape.Location);
			Assert.AreEqual(obj, model.gameObjects[obj.ID]);
			Assert.AreEqual(TypesGameObject.Box, obj.Type);
			Assert.AreEqual(TypesBehaveObjects.Passive, obj.TypeBehave);

		}

		[TestMethod()]
		public void CreateBushTest()
		{
			var location = new PointF(10, 10);
			var model = new RoyalGameModel();
			var obj = BuilderGameObject.CreateBush(model, location);

			var body = obj.Components.GetComponent<TransparentBody>();
			Assert.IsNotNull(body);

			Assert.AreEqual(location, body.Shape.Location);
			Assert.AreEqual(obj, model.gameObjects[obj.ID]);
			Assert.AreEqual(TypesGameObject.Bush, obj.Type);
			Assert.AreEqual(TypesBehaveObjects.Passive, obj.TypeBehave);
		}

		[TestMethod()]
		public void CreateDeathZoneTest()
		{
			var diameter = 15F;
			var model = new RoyalGameModel();
			var obj = BuilderGameObject.CreateDeathZone(model, diameter);

			var body = obj.Components.GetComponent<BodyZone>();
			var damageZone = obj.Components.GetComponent<DamageZone>();

			Assert.IsNotNull(body);
			Assert.IsNotNull(damageZone);

			Assert.AreEqual(obj, model.gameObjects[obj.ID]);
			Assert.AreEqual(TypesGameObject.DeathZone, obj.Type);
			Assert.AreEqual(TypesBehaveObjects.Active, obj.TypeBehave);
		}

		[TestMethod()]
		public void CreateGrenadeTest()
		{
			var x = 10;
			var y = 10;
			var location = new PointF(x, y);
			var model = new RoyalGameModel();
			var obj = BuilderGameObject.CreateGrenade(model, location, new Box2DX.Common.Vec2(x, y), new GrenadeBullet());

			var body = obj.Components.GetComponent<SolidBody>();
			var explosion = obj.Components.GetComponent<Explosion>();

			Assert.IsNotNull(body);
			Assert.IsNotNull(explosion);

			Assert.AreEqual(location, body.Shape.Location);
			Assert.AreEqual(obj, model.gameObjects[obj.ID]);
			Assert.AreEqual(TypesGameObject.Grenade, obj.Type);
			Assert.AreEqual(TypesBehaveObjects.Active, obj.TypeBehave);

			for (int i = 0; i < 10; i++)
			{
				model.MakeStep(i);
			}
			Assert.IsTrue((body.Shape.Location.X - x) > 0);
			Assert.IsTrue((body.Shape.Location.Y - y) > 0);

		}

		[TestMethod()]
		public void CreateStoneTest()
		{
			var location = new PointF(10, 10);
			var model = new RoyalGameModel();
			var obj = BuilderGameObject.CreateStone(model, location);

			var solidBody = obj.Components.GetComponent<SolidBody>();
			Assert.IsNotNull(solidBody);

			Assert.AreEqual(location, solidBody.Shape.Location);
			Assert.AreEqual(obj, model.gameObjects[obj.ID]);
			Assert.AreEqual(TypesGameObject.Stone, obj.Type);
			Assert.AreEqual(TypesBehaveObjects.Passive, obj.TypeBehave);
		}

		[TestMethod()]
		public void CreateTreeTest()
		{
			var location = new PointF(10, 10);
			var model = new RoyalGameModel();
			var obj = BuilderGameObject.CreateTree(model, location);

			var solidBody = obj.Components.GetComponent<SolidBody>();
			Assert.IsNotNull(solidBody);

			Assert.AreEqual(location, solidBody.Shape.Location);
			Assert.AreEqual(obj, model.gameObjects[obj.ID]);
			Assert.AreEqual(TypesGameObject.Tree, obj.Type);
			Assert.AreEqual(TypesBehaveObjects.Passive, obj.TypeBehave);
		}

		[TestMethod()]
		public void CreateGunTest()
		{
			var location = new PointF(10, 10);
			var model = new RoyalGameModel();
			var obj = BuilderGameObject.CreateGun(model, location);

			var solidBody = obj.Components.GetComponent<SolidBody>();
			var shot = obj.Components.GetComponent<Shot>();
			var magazin = obj.Components.GetComponent<Magazin>();

			Assert.IsNotNull(solidBody);
			Assert.IsNotNull(shot);
			Assert.IsNotNull(magazin);

			Assert.AreEqual(location, solidBody.Shape.Location);
			Assert.AreEqual(obj, model.gameObjects[obj.ID]);
			Assert.AreEqual(TypesGameObject.Weapon, obj.Type);
			Assert.AreEqual(TypesWeapon.Gun, (obj as IWeapon).TypeWeapon);
			Assert.AreEqual(TypesBehaveObjects.Active, obj.TypeBehave);
		}

		[TestMethod()]
		public void CreateAssaultRiffleTest()
		{
			var location = new PointF(10, 10);
			var model = new RoyalGameModel();
			var obj = BuilderGameObject.CreateAssaultRiffle(model, location);

			var solidBody = obj.Components.GetComponent<SolidBody>();
			var shot = obj.Components.GetComponent<Shot>();
			var magazin = obj.Components.GetComponent<Magazin>();

			Assert.IsNotNull(solidBody);
			Assert.IsNotNull(shot);
			Assert.IsNotNull(magazin);

			Assert.AreEqual(location, solidBody.Shape.Location);
			Assert.AreEqual(obj, model.gameObjects[obj.ID]);
			Assert.AreEqual(TypesGameObject.Weapon, obj.Type);
			Assert.AreEqual(TypesWeapon.AssaultRifle, (obj as IWeapon).TypeWeapon);
			Assert.AreEqual(TypesBehaveObjects.Active, obj.TypeBehave);
		}

		[TestMethod()]
		public void CreateShotGunTest()
		{
			var location = new PointF(10, 10);
			var model = new RoyalGameModel();
			var obj = BuilderGameObject.CreateShotGun(model, location);

			var solidBody = obj.Components.GetComponent<SolidBody>();
			var shot = obj.Components.GetComponent<Shot>();
			var magazin = obj.Components.GetComponent<Magazin>();

			Assert.IsNotNull(solidBody);
			Assert.IsNotNull(shot);
			Assert.IsNotNull(magazin);

			Assert.AreEqual(location, solidBody.Shape.Location);
			Assert.AreEqual(obj, model.gameObjects[obj.ID]);
			Assert.AreEqual(TypesGameObject.Weapon, obj.Type);
			Assert.AreEqual(TypesWeapon.ShotGun, (obj as IWeapon).TypeWeapon);
			Assert.AreEqual(TypesBehaveObjects.Active, obj.TypeBehave);
		}

		[TestMethod()]
		public void CreateGrenadeCollectionTest()
		{
			var location = new PointF(10, 10);
			var model = new RoyalGameModel();
			var obj = BuilderGameObject.CreateGrenadeCollection(model, location);

			var solidBody = obj.Components.GetComponent<SolidBody>();
			var throw_ = obj.Components.GetComponent<Throw>();
			var magazin = obj.Components.GetComponent<Magazin>();

			Assert.IsNotNull(solidBody);
			Assert.IsNotNull(throw_);
			Assert.IsNotNull(magazin);

			Assert.AreEqual(location, solidBody.Shape.Location);
			Assert.AreEqual(obj, model.gameObjects[obj.ID]);
			Assert.AreEqual(TypesGameObject.Weapon, obj.Type);
			Assert.AreEqual(TypesWeapon.GrenadeCollection, (obj as IWeapon).TypeWeapon);
			Assert.AreEqual(TypesBehaveObjects.Active, obj.TypeBehave);
		}

		[TestMethod()]
		public void CreateGamerTest()
		{
			var location = new PointF(10, 10);
			var model = new RoyalGameModel();
			var obj = BuilderGameObject.CreateGamer(model, location);

			var solidBody = obj.Components.GetComponent<SolidBody>();
			var healthy = obj.Components.GetComponent<Healthy>();
			var collector = obj.Components.GetComponent<Collector>();
			var currentWeapon = obj.Components.GetComponent<CurrentWeapon>();
			var statistics = obj.Components.GetComponent<Statistics>();

			Assert.IsNotNull(solidBody);
			Assert.IsNotNull(healthy);
			Assert.IsNotNull(collector);
			Assert.IsNotNull(currentWeapon);
			Assert.IsNotNull(statistics);

			Assert.AreEqual(obj, model.Players.First());
			Assert.AreEqual(obj, model.gameObjects[obj.ID]);
			Assert.AreEqual(location, solidBody.Shape.Location);
			Assert.AreEqual(location, (obj as Gamer).Location);

			Assert.AreEqual(TypesGameObject.Player, obj.Type);
			Assert.AreEqual(false, (obj as Gamer).Destroyed);
			Assert.AreEqual(TypesBehaveObjects.Active, obj.TypeBehave);
		}
	}
}