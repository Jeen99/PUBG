using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleRoayleServer;
using CommonLibrary.GameMessages;
using System.Drawing;
using ServerTest.Common;
using CommonLibrary.CommonElements;

namespace ServerTest.ComponentsTest
{
	[TestClass]
	public class BodyZoneTest
	{
		[TestMethod]
		public void Test_SendMessage()
		{
			var model = new MockRoyalGameModel();
			GameObject zone = new GameObject(model, TypesGameObject.DeathZone, TypesBehaveObjects.Passive);
			BodyZone bodyZone = new BodyZone(zone, 400);
			zone.Components.Add(bodyZone);
			zone.Setup();
			//model.AddOrUpdateGameObject(zone);

			PointF startLocation = bodyZone.Location;
			float radius = bodyZone.Radius;

			zone.Update(new TimeQuantPassed(100));

			Assert.AreEqual(1, model.OutgoingMessages.Count);
			Assert.AreEqual(startLocation, bodyZone.Location);
			Assert.AreEqual(radius, bodyZone.Radius);
		}

		[TestMethod]
		public void Test_ChangeLocation()
		{
			var model = new MockRoyalGameModel();
			GameObject zone = new GameObject(model, TypesGameObject.DeathZone, TypesBehaveObjects.Passive);
			BodyZone bodyZone = new BodyZone(zone, 500);
			zone.Components.Add(bodyZone);
			zone.Setup();

			PointF startLocation = bodyZone.Location;
			float radius = bodyZone.Radius;

			zone.Update(new TimeQuantPassed(30001));

			//Assert.AreEqual(2, model.OutgoingMessages.Count);
			Assert.AreNotEqual(startLocation, bodyZone.Location);
			Assert.AreEqual(250, bodyZone.Radius);


			startLocation = bodyZone.Location;
			radius = bodyZone.Radius;
			zone.Update(new TimeQuantPassed(30001));
			Assert.AreEqual(model.OutgoingMessages.Count, 4);
			Assert.AreNotEqual(startLocation, bodyZone.Location);
			Assert.AreEqual(radius * 0.6f, bodyZone.Radius);
		}
	}
}
