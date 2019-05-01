using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleRoayleServer;
using CommonLibrary.CommonElements;

namespace ServerTest
{
	[TestClass]
	public class DictionaryComponentsTest
	{
		[TestMethod]
		public void Test_GetComponent()
		{
			DictionaryComponent  dictionary = new DictionaryComponent();
			IComponent magazin = new Magazin(new StubWeapon(), TypesWeapon.Gun, 500, 1000, 8);
			magazin.Setup();
			dictionary.Add(magazin);
			Magazin thisMagazin = dictionary.GetComponent<Magazin>();
			Assert.AreEqual(magazin, thisMagazin);
		}
	}
}
