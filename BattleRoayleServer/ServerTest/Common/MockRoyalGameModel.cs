﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleRoayleServer;
using Box2DX.Collision;
using Box2DX.Dynamics;
using CommonLibrary;
using CommonLibrary.CommonElements;

namespace ServerTest.Common
{
	class MockRoyalGameModel : IGameModel, IModelForComponents
	{
		public IList<IPlayer> Players => throw new NotImplementedException();

		public IMessage RoomState => throw new NotImplementedException();

		public IMessage FullRoomState => throw new NotImplementedException();

		public World Field { get; set; }

#pragma warning disable CS0067
		public event HappenedEndGame Event_HappenedEndGame;
#pragma warning restore CS0067
		public Queue<IMessage> IncomingMessages { get; set; } = new Queue<IMessage>();
		public Queue<IMessage> OutgoingMessages { get; set; } = new Queue<IMessage>();

		public IGameObject DeathZone => throw new NotImplementedException();

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

		public List<SolidBody> GetMetedObjects(Segment ray)
		{
			throw new NotImplementedException();
		}
	}
}
