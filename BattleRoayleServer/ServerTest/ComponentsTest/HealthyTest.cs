using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleRoayleServer;
using CSInteraction.ProgramMessage;

namespace ServerTest.ComponentsTest
{
	[TestClass]
	public class HealthyTest
	{
		[TestMethod]
		public void Test_UpdateComponent_GotDamaged()
		{
			IHealthy healthy = new Healthy(new StubPlayer());
			healthy.Setup();
			healthy.UpdateComponent(new GotDamage(8));
			Assert.AreEqual(healthy.HP, 92);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void Test_ErrorCreateHealthy()
		{
			IHealthy healthy = new Healthy(null);
		}

		[TestMethod]
		public void Test_CreateHealthy()
		{
			IHealthy healthy = new Healthy(new StubPlayer());
		}

		[TestMethod]
		public void Test_HealthyState()
		{
			IHealthy healthy = new Healthy(new StubPlayer());
			Assert.IsNotNull(healthy.State);
		}
	}
}
