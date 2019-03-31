using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleRoayleServer;
using CSInteraction.ProgramMessage;
using CSInteraction.Common;

namespace ServerTest.ComponentsTest
{
	[TestClass]
	public class CurrentWeaponTest
	{
		[TestMethod]
		public void Test_CreateCurrentWeapon()
		{
			var weapon = new StubPlayer();
			weapon.Components.Add(new SolidBody(weapon));
			weapon.Components.Add(new Collector(weapon));
			ICurrentWeapon currentWeapon = new CurrentWeapon(weapon);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void Test_ErrorCreateCurrentWeapon1()
		{
			ICurrentWeapon currentWeapon = new CurrentWeapon(new StubWeapon());
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void Test_ErrorCreateCurrentWeapon2()
		{
			ICurrentWeapon currentWeapon = new CurrentWeapon(null);
		}

		[TestMethod]
		public void Test_StateCurrentWeaponNull()
		{
			var weapon = new StubPlayer();
			weapon.Components.Add(new SolidBody(weapon));
			weapon.Components.Add(new Collector(weapon));
			ICurrentWeapon currentWeapon = new CurrentWeapon(weapon);
			Assert.IsNull(currentWeapon.State);
		}

	}
}
