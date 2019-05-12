using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleRoayleServer;
using Box2DX.Dynamics;
using CommonLibrary;
using CommonLibrary.CommonElements;

namespace ServerTest.Common
{
	class MockRoyalGameModel : IGameModel, IModelForComponents
	{
		public IList<IPlayer> Players => throw new NotImplementedException();

		public DeathZone Zone => throw new NotImplementedException();

		public IMessage RoomState => throw new NotImplementedException();

		public IMessage FullRoomState => throw new NotImplementedException();

		public World Field { get; set; }

		public event HappenedEndGame Event_HappenedEndGame;

		public Queue<IMessage> IncomingMessages { get; set; } = new Queue<IMessage>();
		public Queue<IMessage> OutgoingMessages { get; set; } = new Queue<IMessage>();

		public void AddIncomingMessage(IMessage message)
		{
			IncomingMessages.Enqueue(message);
		}

		public void AddOrUpdateGameObject(IGameObject gameObject)
		{
			throw new NotImplementedException();
		}

		public void AddOutgoingMessage(IMessage message)
		{
			OutgoingMessages.Enqueue(message);
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		public IMessage GetOutgoingMessage()
		{
			throw new NotImplementedException();
		}

		public void MakeStep(int passedTime)
		{
			throw new NotImplementedException();
		}

		public void RemoveGameObject(IGameObject gameObject)
		{
			throw new NotImplementedException();
		}
	}
}
