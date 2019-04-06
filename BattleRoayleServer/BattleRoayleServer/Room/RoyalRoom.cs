using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleRoayleServer
{
    public class RoyalRoom : IRoom
    {
        public INetwork NetworkLogic { get; private set; }

        public IRoomLogic GameLogic { get; private set; }

        public RoyalRoom(IList<QueueGamer> gamers)
        {
			GameLogic = new RoyalRoomLogic(gamers.Count);
			NetworkLogic = new RoomNetwork(gamers, GameLogic);
			GameLogic.EventRoomLogicEndWork += GameLogic_EventRoomLogicEndWork; ;
        }

		private void GameLogic_EventRoomLogicEndWork(IRoomLogic roomLogic)
		{
			EventRoomEndWork?.Invoke(this);
		}

		public event RoomEndWork EventRoomEndWork;

		public void StartRoom()
		{
			GameLogic.Start();
			NetworkLogic.Start();
		}

		public void Dispose()
		{
			NetworkLogic.Dispose();
			GameLogic.Dispose();
		}
	}
}