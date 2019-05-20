using BattleRoyalServer;
using CommonLibrary;
using CommonLibrary.CommonElements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerTest.Common
{
	class MockPlayer : IGameObject, IPlayer
	{
		public DictionaryComponent Components => new DictionaryComponent();

		public bool Destroyed { get; set; } = false;

		public ulong ID { get; set; } = 1;

		public IModelForComponents Model { get; set; } = new MockRoyalGameModel();

		public IMessage State => throw new NotImplementedException();

		public TypesGameObject Type => throw new NotImplementedException();

		public TypesBehaveObjects TypesBehave => throw new NotImplementedException();

		public PointF Location => throw new NotImplementedException();

		public TypesBehaveObjects TypeBehave => TypesBehaveObjects.Active;

		public IGameObject Owner { get; set; }

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
		public event ReceivedMessage Received_Update;

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		public void SetDestroyed()
		{
			Destroyed = true;
		}

		public void Setup()
		{
			//throw new NotImplementedException();
		}

		public void Update(IMessage msg)
		{
			Received_Update?.Invoke(msg);
		}

		#region UpdateEvent
		public void Update_ChoiceWeapon(IMessage msg)
		{
			Received_ChoiceWeapon.Invoke(msg);
		}

		public void Update_GamerDied(IMessage msg)
		{
			Received_GamerDied.Invoke(msg);
		}

		public void Update_GotDamage(IMessage msg)
		{
			Received_GotDamage.Invoke(msg);
		}

		public void Update_GoTo(IMessage msg)
		{
			Received_GoTo.Invoke(msg);
		}

		public void Update_MakeShot(IMessage msg)
		{
			Received_MakeShot.Invoke(msg);
		}

		public void Update_PlayerTurn(IMessage msg)
		{
			Received_PlayerTurn.Invoke(msg);
		}

		public void Update_MakeReloadWeapon(IMessage msg)
		{
			Received_MakeReloadWeapon.Invoke(msg);
		}

		public void Update_TryPickUp(IMessage msg)
		{
			Received_TryPickUp.Invoke(msg);
		}

		public void Update_DeletedInMap(IMessage msg)
		{
			Received_DeletedInMap.Invoke(msg);
		}

		public void Update_TimeQuantPassed(IMessage msg)
		{
			Received_TimeQuantPassed.Invoke(msg);
		}

		public void Update_AddWeapon(IMessage msg)
		{
			Received_AddWeapon.Invoke(msg);
		}

		public void Update_MakedKill(IMessage msg)
		{
			Received_MakedKill.Invoke(msg);
		}
		#endregion
	}
}
