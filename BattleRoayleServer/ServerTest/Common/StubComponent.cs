using BattleRoayleServer;
using CommonLibrary;
using System;

namespace ServerTest.Common
{
	class StubComponent : IComponent
	{
		public IGameObject Parent => throw new NotImplementedException();

		public IMessage State => throw new NotImplementedException();

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		public void Setup()
		{
			throw new NotImplementedException();
		}
	}
}
