using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleRoayleServer;
using System.Drawing;
using Box2DX.Common;
using System.Diagnostics;
using CommonLibrary.GameMessages;

namespace ServerTest.ComponentsTest
{
	[TestClass]
	public class ThrowTest
	{
		[TestMethod]
		public void Test_ThrowGrenade()
		{

			RoyalGameModel model = new RoyalGameModel();
			var grenade = BuilderGameObject.CreateGrenade(model, new PointF(50, 50), new Vec2(0, 10), new GrenadeBullet());
			SolidBody bodyGrenade = grenade.Components.GetComponent<SolidBody>();
			PointF start = bodyGrenade.Shape.Location;
			//прошла 1 секунда
			model.Field.Step(1f, 6, 3);
			grenade.Update(new TimeQuantPassed(1000));
			PointF step1 = bodyGrenade.Shape.Location;
			Assert.AreNotEqual(start, step1);
			//прошла 2 секунда
			model.Field.Step(1f, 6, 3);
			grenade.Update(new TimeQuantPassed(1000));
			PointF step2 = bodyGrenade.Shape.Location;
			Assert.AreNotEqual(step1, step2);
			//прошла 3 секунда
			model.Field.Step(1f, 6, 3);
			grenade.Update(new TimeQuantPassed(1000));
			PointF step3 = bodyGrenade.Shape.Location;
			Assert.AreNotEqual(step2, step3);
			//прошла 4 секунда
			model.Field.Step(1f, 6, 3);
			grenade.Update(new TimeQuantPassed(1000));
			PointF step4 = bodyGrenade.Shape.Location;
			Assert.AreNotEqual(step3, step4);
		}

		[TestMethod]
		public void Test_ThrowGrenadeAndDamage()
		{
			RoyalGameModel model = new RoyalGameModel();
			Gamer player = BuilderGameObject.CreateGamer(model, new PointF(70, 100));
			Healthy healthy = player.Components.GetComponent<Healthy>();
			float startHp = healthy.HP;
			//player.Setup();

			var grenade = BuilderGameObject.CreateGrenade(model, new PointF(50, 50), new Vec2(20, 50), new GrenadeBullet());
			SolidBody bodyGrenade = grenade.Components.GetComponent<SolidBody>();
			PointF startStep = bodyGrenade.Shape.Location;

			//смотрим как движется граната
			for (int i = 0; i < 44; i++)
			{
				model.Field.Step(0.1f, 6, 3);
				grenade.Update(new TimeQuantPassed(100));
				player.Update(new TimeQuantPassed(100));
				PointF endStep = bodyGrenade.Shape.Location;
				Debug.WriteLine($"{endStep.X}:{endStep.Y}");
				Assert.AreEqual(startHp, healthy.HP);
			}

			model.Field.Step(0.1f, 6, 3);
			grenade.Update(new TimeQuantPassed(100));
			player.Update(new TimeQuantPassed(100));
			Assert.AreEqual(startHp - 50, healthy.HP);
		}
	}
}
