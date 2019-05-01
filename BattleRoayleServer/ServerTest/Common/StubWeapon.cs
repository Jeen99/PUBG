using BattleRoayleServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.CommonElements;
using CommonLibrary;

namespace ServerTest
{
	class StubWeapon : IWeapon
	{
		public IGameObject Holder { get ; set ; }

		public TypesWeapon TypeWeapon { get; } = TypesWeapon.Gun;

		public DictionaryComponent Components { get; } = new DictionaryComponent();

		public bool Destroyed { get; } = false;

		public ulong ID { get; } = 1;

		public IModelForComponents Model { get; } = null;

		public IMessage State { get; } = null;

		public TypesGameObject Type { get; } = TypesGameObject.Weapon;

		public TypesBehaveObjects TypesBehave { get; } = TypesBehaveObjects.Passive;

		public event ReceivedMessage Received_ChoiceWeapon;
		public event ReceivedMessage Received_GamerDied;
		public event ReceivedMessage Received_GotDamage;
		public event ReceivedMessage Received_GoTo;
		public event ReceivedMessage Received_MakeShot;
		public event ReceivedMessage Received_PlayerTurn;
		public event ReceivedMessage Received_MakeReloadWeapon;
		public event ReceivedMessage Received_TryPickUp;
		public event ReceivedMessage Received_DeletedInMap;
		public event ReceivedMessage Received_TimeQuantPassed;
		public event ReceivedMessage Received_AddWeapon;
		public event ReceivedMessage Received_MakedKill;

		public void Dispose()
		{

		}

		public void Update(IMessage msg)
		{
			
		}

		public void SetDestroyed()
		{
			throw new NotImplementedException();
		}

		public void Setup()
		{
			throw new NotImplementedException();
		}

	}
}
