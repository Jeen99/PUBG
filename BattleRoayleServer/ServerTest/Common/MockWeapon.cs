using BattleRoayleServer;
using CommonLibrary;
using CommonLibrary.CommonElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerTest.Common
{
	internal class MockWeapon : Weapon
	{
		public MockWeapon(IModelForComponents model) : base(model)
		{
		}

		public MockWeapon() : base(new MockRoyalGameModel())
		{
		}

		public override TypesBehaveObjects TypesBehave => throw new NotImplementedException();

		public override TypesGameObject Type => throw new NotImplementedException();
	}
}
