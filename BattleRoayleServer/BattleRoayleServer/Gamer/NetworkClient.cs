using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSInteraction.Server;

namespace BattleRoayleServer
{
	public class NetworkClient : INetworkClient
	{
		private IPlayer gamerRoomLogic;

		public string Nick { get; private set; }

		public ServerClient Gamer { get; private set; }

		public string Password { get; private set; }
		
	}
}