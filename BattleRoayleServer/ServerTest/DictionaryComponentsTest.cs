using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleRoayleServer;
using CommonLibrary.CommonElements;
using ServerTest.Common;

namespace ServerTest
{
	[TestClass]
	public class DictionaryComponentsTest
	{
		[TestMethod]
		public void Test_GetComponent()
		{
			DictionaryComponent dictionary = new DictionaryComponent();
			IComponent component = new StubComponent();
			dictionary.Add(component);

			// не обрабатывавает имключение, если такого компонента нет в словаре
			var thisComponent = dictionary.GetComponent(typeof(StubComponent));
			Assert.AreSame(component, thisComponent);
		}

		[TestMethod]
		public void Test_GetComponentGeneric_Non_existing_component()
		{
			DictionaryComponent  dictionary = new DictionaryComponent();

			var thisComponent = dictionary.GetComponent<StubComponent>();
			Assert.IsNull(thisComponent);
		}

		[TestMethod]
		public void Test_GetComponentGeneric()
		{
			DictionaryComponent dictionary = new DictionaryComponent();
			IComponent component = new StubComponent();
			dictionary.Add(component);

			var thisComponent = dictionary.GetComponent<StubComponent>();
			Assert.AreSame(component, thisComponent);
		}

		[TestMethod]
		public void Test_GetComponentByString()
		{
			DictionaryComponent dictionary = new DictionaryComponent();
			IComponent component = new StubComponent();
			dictionary.Add(component);

			var thisComponent = dictionary.GetComponent("StubComponent");
			Assert.AreSame(component, thisComponent);
		}

		[TestMethod]
		public void Test_GetComponentsGeneric()
		{
			DictionaryComponent dictionary = new DictionaryComponent();
			IComponent component1 = new StubComponent();
			IComponent component2 = new MockComponent();

			dictionary.Add(component1);
			dictionary.Add(component2);

			var receivedComponents = dictionary.GetComponents<IComponent>();

			IComponent ReceivedComponent1 = receivedComponents[0];
			IComponent ReceivedComponent2 = receivedComponents[1];

			Assert.AreEqual(2, receivedComponents.Length);
			Assert.AreSame(component1, ReceivedComponent1);
			Assert.AreSame(component2, ReceivedComponent2);
		}

		[TestMethod]
		[ExpectedException(typeof(IndexOutOfRangeException))]
		public void Test_GetComponentByString_Non_existing_component()
		{
			DictionaryComponent dictionary = new DictionaryComponent();
			var thisComponent = dictionary.GetComponent("StubComponent");
		}

		[TestMethod]
		public void Test_RemoveComponent()
		{
			DictionaryComponent dictionary = new DictionaryComponent();
			IComponent component = new StubComponent();
			dictionary.Remove(component);

			var thisComponent = dictionary.GetComponent<StubComponent>();
			Assert.AreNotSame(thisComponent, component);
		}

		[TestMethod]
		public void Test_RemoveComponentGeneric()
		{
			DictionaryComponent dictionary = new DictionaryComponent();
			IComponent component = new StubComponent();
			dictionary.Remove<StubComponent>();

			var thisComponent = dictionary.GetComponent<StubComponent>();
			Assert.AreNotSame(thisComponent, component);
		}
	}
}
