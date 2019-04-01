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
			var player = new StubPlayer();
			player.Components.Add(new SolidBody(player));
			player.Components.Add(new Collector(player));
			player.Setup();
			ICurrentWeapon currentWeapon = new CurrentWeapon(player);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void Test_ErrorSetupCurrentWeapon1()
		{
			ICurrentWeapon currentWeapon = new CurrentWeapon(new StubWeapon());
			currentWeapon.Setup();
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void Test_ErrorCreateCurrentWeapon()
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
