using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleRoayleServer;
using CSInteraction.Common;
using CSInteraction.ProgramMessage;

namespace ServerTest
{
	class StubPlayer : IGameObject
	{
		public DictionaryComponent Components { get; } = new DictionaryComponent();

		public bool Destroyed { get; } = false;

		public ulong ID { get; } = 1;

		public IModelForComponents Model { get; } = new RoyalGameModel();

		public IMessage State { get; } = null;

		public TypesGameObject Type { get; } = TypesGameObject.Player;

		public TypesBehaveObjects TypesBehave { get; } = TypesBehaveObjects.Active;

		public TypesWeapon TypeWeapon { get; } = TypesWeapon.Gun;

		public event GameObjectDeleted EventGameObjectDeleted;

		public void Dispose()
		{
			return;
		}

		public void SendMessage(IMessage msg)
		{
			return;
		}

		public void Setup()
		{
			foreach (IComponent item in Components)
			{
				item.Setup();
			}
		}

		public void Update(TimeQuantPassed quantPassed = null)
		{
			return;
		}
	}
}
