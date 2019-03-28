using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleRoayleServer;
using CSInteraction.Common;

namespace ServerTest
{
	[TestClass]
	public class DictionaryComponentsTest
	{
		[TestMethod]
		public void Test_GetComponentAnotherInterface()
		{
			DictionaryComponent  dictionary = new DictionaryComponent();
			IComponent magazin = new Magazin(new StubWeapon(), TypesWeapon.Gun, 500, 1000);
			dictionary.Add(magazin);
			IMagazin thisMagazin = dictionary.GetComponent<Magazin>();
			Assert.AreEqual(magazin, thisMagazin);
		}
	}
}
