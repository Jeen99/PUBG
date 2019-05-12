using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleRoayleServer;
using CommonLibrary.GameMessages;
using System.Drawing;
using ServerTest.Common;

namespace ServerTest.ComponentsTest
{
	[TestClass]
	public class BodyZoneTest
	{
		[TestMethod]
		public void Test_SendMessage()
		{
			var model = new MockRoyalGameModel();
			DeathZone zone = new DeathZone(model, 500);
			zone.Setup();
			BodyZone bodyZone = zone.Components.GetComponent<BodyZone>();

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
			var model = new RoyalGameModel();
			DeathZone zone = new DeathZone(model, 500);
			zone.Setup();
			BodyZone bodyZone = zone.Components.GetComponent<BodyZone>();

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
