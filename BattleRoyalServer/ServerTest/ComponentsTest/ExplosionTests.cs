using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleRoyalServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerTest.Common;
using CommonLibrary.CommonElements;
using CommonLibrary.GameMessages;

namespace ServerTest.ComponentsTest
{
	[TestClass()]
	public class ExplosionTests
	{
		[TestMethod()]
		[ExpectedException(typeof(Exception))]
		public void ExplosionTest()
		{
			var model = new MockRoyalGameModel();
			var obj = new GameObject(model, TypesGameObject.Grenade, TypesBehaveObjects.Active);
			var explosion = new Explosion(obj, new GrenadeBullet());
			explosion.Setup();
		}

		[TestMethod()]
		public void SetupTest()
		{
			var model = new RoyalGameModel();
			var obj = new GameObject(model, TypesGameObject.Grenade, TypesBehaveObjects.Active);
			var body = new SolidBody(obj);
			obj.Components.Add(body);
			var explosion = new Explosion(obj, new GrenadeBullet());
			obj.Components.Add(explosion);

			Assert.IsFalse(explosion.Parent.Destroyed);
			obj.Setup();

			obj.Update(new TimeQuantPassed(10000));
			Assert.IsTrue(explosion.Parent.Destroyed);
		}

		[TestMethod()]
		public void DisposeTest()
		{
			var model = new RoyalGameModel();
			var obj = new GameObject(model, TypesGameObject.Grenade, TypesBehaveObjects.Active);
			var body = new SolidBody(obj);
			obj.Components.Add(body);
			var explosion = new Explosion(obj, new GrenadeBullet());
			obj.Components.Add(explosion);

			Assert.IsFalse(explosion.Parent.Destroyed);
			obj.Setup();

			explosion.Dispose();
			obj.Update(new TimeQuantPassed(10000));
			Assert.IsFalse(explosion.Parent.Destroyed);
		}
	}
}