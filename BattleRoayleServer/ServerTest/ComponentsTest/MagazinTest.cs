using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleRoayleServer;
using CSInteraction.Common;
using System.Threading;
using CSInteraction.ProgramMessage;

namespace ServerTest.ComponentsTest
{
	[TestClass]
	public class MagazinTest
	{
		[TestMethod]
		public void Test_CreateMagazin()
		{
			IMagazin magazin = new Magazin(new StubWeapon(), TypesWeapon.Gun, 500, 3000);
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void Test_ErrorCreateMagazin()
		{
			IMagazin magazin = new Magazin(null, TypesWeapon.Gun, 500, 3000);
		}

		[TestMethod]
		public void Test_StateMagazin()
		{
			IMagazin magazin = new Magazin(new StubWeapon(), TypesWeapon.Gun, 500, 3000);
			Assert.IsNotNull(magazin.State);
		}

		[TestMethod]
		public void Test_GetBullet()
		{
			IMagazin magazin = new Magazin(new StubWeapon(), TypesWeapon.Gun, 50, 300);
			//делаем 8 выстрелов
			for (int i = 0; i < 8; i++)
			{
				Assert.IsNotNull(magazin.GetBullet());
				Assert.IsNull(magazin.GetBullet());
				Thread.Sleep(75);
			}
			//перезаряжаем 
			Assert.IsNull(magazin.GetBullet());
			Thread.Sleep(350);
			Assert.IsNotNull(magazin.GetBullet());
		}

		[TestMethod]
		public void Test_UpdateComponent_MakeReload()
		{
			IMagazin magazin = new Magazin(new StubWeapon(), TypesWeapon.Gun, 50, 300);
			magazin.UpdateComponent(new MakeReloadWeapon());
			Assert.IsNull(magazin.GetBullet());
			Thread.Sleep(330);
			Assert.IsNotNull(magazin.GetBullet());
		}
	}
}
