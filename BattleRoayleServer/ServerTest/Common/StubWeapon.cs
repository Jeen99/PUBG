using BattleRoayleServer;
using CSInteraction.Common;
using CSInteraction.ProgramMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerTest
{
	class StubWeapon : IWeapon
	{
		public IGameObject Holder { get ; set ; }

		public TypesWeapon TypeWeapon { get; } = TypesWeapon.Gun;

		public DictionaryComponent Components { get; } = new DictionaryComponent();

		public bool Destroyed { get; } = false;

		public ulong ID { get; } = 1;

		public IGameModel Model { get; } = null;

		public IMessage State { get; } = null;

		public TypesGameObject Type { get; } = TypesGameObject.Weapon;

		public TypesBehaveObjects TypesBehave { get; } = TypesBehaveObjects.Passive;

		public event GameObjectDeleted EventGameObjectDeleted;

		public void Dispose()
		{

		}

		public void SendMessage(IMessage msg)
		{
			
		}

		public void Update(TimeQuantPassed quantPassed = null)
		{

		}
	}
}
