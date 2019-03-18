using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSInteraction.Server;

namespace BattleRoayleServer
{
    public interface INetworkClient
    {
        string Nick { get; }
        ServerClient Gamer { get; }
		string Password { get; }
		event GamerIsLoaded Event_GamerIsLoaded;
	}
}