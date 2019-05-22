using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleRoyalServer;

namespace ServerTest.ComponentsTest
{
	[TestClass]
	public class CurrentWeaponTest
	{
		[TestMethod]
		public void Test_CreateCurrentWeapon()
		{
			var player = new StubPlayer();
			player.Components.Add(new SolidBody(player));
			player.Components.Add(new Collector(player));
			player.Setup();
			CurrentWeapon currentWeapon = new CurrentWeapon(player);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void Test_ErrorSetupCurrentWeapon1()
		{
			CurrentWeapon currentWeapon = new CurrentWeapon(new StubWeapon());
			currentWeapon.Setup();
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void Test_ErrorCreateCurrentWeapon()
		{
			CurrentWeapon currentWeapon = new CurrentWeapon(null);
		}

		[TestMethod]
		public void Test_StateCurrentWeaponNull()
		{
			var weapon = new StubPlayer();
			weapon.Components.Add(new SolidBody(weapon));
			weapon.Components.Add(new Collector(weapon));
			CurrentWeapon currentWeapon = new CurrentWeapon(weapon);
			Assert.IsNull(currentWeapon.State);
		}

	}
}
