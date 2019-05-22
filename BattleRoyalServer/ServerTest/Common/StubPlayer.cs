using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleRoyalServer;
using CommonLibrary;
using CommonLibrary.CommonElements;

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

		IMessage IGameObject.State => throw new NotImplementedException();

		TypesGameObject IGameObject.Type => throw new NotImplementedException();

		public TypesBehaveObjects TypeBehave => throw new NotImplementedException();

		public IGameObject Owner { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
#pragma warning disable CS0067
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
#pragma warning restore CS0067
		public void Dispose()
		{
			return;
		}

		public void Update(IMessage msg)
		{
			return;
		}

		public void SetDestroyed()
		{
			throw new NotImplementedException();
		}

		public void Setup()
		{
			foreach (IComponent item in Components)
			{
				item.Setup();
			}
		}
	}
}