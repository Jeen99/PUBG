using BattleRoyalServer;
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
		public MockWeapon(IModelForComponents model, TypesGameObject typeGameObject, TypesBehaveObjects typeBehaveObject, TypesWeapon typeWeapon) : base(model, typeGameObject, typeBehaveObject, typeWeapon)
		{
		}
	}
}
