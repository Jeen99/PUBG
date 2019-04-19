using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleRoayleServer;
using CSInteraction.ProgramMessage;
using System.Drawing;

namespace ServerTest.ComponentsTest
{
	[TestClass]
	public class BodyZoneTest
	{
		
		[TestMethod]
		public void Test_SendMessage()
		{
			var model = new RoyalGameModel();
			DeathZone zone = new DeathZone(model, 500);
			BodyZone bodyZone = zone.Components.GetComponent<BodyZone>();

			PointF startLocation = bodyZone.Location;
			float radius = bodyZone.Radius;

			bodyZone.UpdateComponent(new TimeQuantPassed(1000));

			Assert.AreEqual(model.HappenedEvents.Count, 1);
			Assert.AreEqual(startLocation, bodyZone.Location);
			Assert.AreEqual(radius, bodyZone.Radius);
		}

		[TestMethod]
		public void Test_ChangeLocation()
		{

			var model = new RoyalGameModel();
			DeathZone zone = new DeathZone(model, 500);
			BodyZone bodyZone = zone.Components.GetComponent<BodyZone>();

			PointF startLocation = bodyZone.Location;
			float radius = bodyZone.Radius;

			bodyZone.UpdateComponent(new TimeQuantPassed(30001));

			Assert.AreEqual(model.HappenedEvents.Count, 2);
			Assert.AreNotEqual(startLocation, bodyZone.Location);
			Assert.AreEqual(250, bodyZone.Radius);


			startLocation = bodyZone.Location;
			radius = bodyZone.Radius;
			bodyZone.UpdateComponent(new TimeQuantPassed(30001));
			Assert.AreEqual(model.HappenedEvents.Count, 4);
			Assert.AreNotEqual(startLocation, bodyZone.Location);
			Assert.AreEqual(radius * 0.6f, bodyZone.Radius);
		}
	}
}
