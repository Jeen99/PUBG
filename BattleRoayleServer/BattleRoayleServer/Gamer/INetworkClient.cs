using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSInteraction.Server;
using System.Drawing;

namespace BattleRoayleServer
{
    public interface INetworkClient
    {
        string Nick { get; }

        ServerClient Client { get; }
		RectangleF VisibleArea { get; }
		string Password { get; }
		event GamerIsLoaded Event_GamerIsLoaded;
		event NetworkClientEndWork EventNetworkClientEndWork;
		event NetorkClientDisconnect EventNetorkClientDisconnect;
		void Dispose();
	    IPlayer Player { get; }
	}
}