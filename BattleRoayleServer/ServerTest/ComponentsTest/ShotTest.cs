using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleRoayleServer;

namespace ServerTest.ComponentsTest
{
	[TestClass]
	public class ShotTest
	{
		[TestMethod]
		[ExpectedException(typeof(Exception))] 
		public void Test_ErrorCreateShot1()
		{

			Shot shot = new Shot(new StubWeapon());
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void Test_ErrorCreateShot2()
		{

			Shot shot = new Shot(null);
		}

		[TestMethod]
		public void Test_CreateShot()
		{
			var weapon = new StubWeapon();
			weapon.Components.Add(new StubMagazin(weapon));
			Shot shot = new Shot(weapon);
		}
	}
}
