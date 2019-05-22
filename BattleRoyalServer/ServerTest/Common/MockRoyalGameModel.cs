using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleRoyalServer;
using Box2DX.Collision;
using Box2DX.Dynamics;
using CommonLibrary;
using CommonLibrary.CommonElements;

namespace ServerTest.Common
{
	class MockRoyalGameModel : IGameModel, IModelForComponents
	{
		public IList<IPlayer> Players { get; set; } = new List<IPlayer>();

		public IMessage RoomState { get; set; }

		public IMessage FullRoomState { get; set; }

		public World Field { get; set; }

#pragma warning disable CS0067
		public event HappenedEndGame Event_HappenedEndGame;
#pragma warning restore CS0067
		public Queue<IMessage> incomingMessages = new Queue<IMessage>();
		public Queue<IMessage> outgoingMessages = new Queue<IMessage>();

		public IGameObject DeathZone { get; set; }

		public void AddIncomingMessage(IMessage message)
		{
			incomingMessages.Enqueue(message);
		}

		public void AddOrUpdateGameObject(IGameObject gameObject)
		{
			throw new NotImplementedException();
		}

		public void AddOutgoingMessage(IMessage message)
		{
			outgoingMessages.Enqueue(message);
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

		public List<SolidBody> GetMetedObjects(Segment ray)
		{
			throw new NotImplementedException();
		}
	}
}
