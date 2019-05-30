using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSInteraction.Server;
using System.Drawing;
using CommonLibrary.GameMessages;
using CommonLibrary;

namespace BattleRoyalServer
{
    public interface INetworkClient
    {
	    IPlayer Player { get; }
	    RectangleF VisibleArea { get; }

		string Nick { get; }
        string Password { get; }

		void SendMessgaeToClient(IMessage msg);
		void SaveStatistics(IMessage msg);
		void Dispose();

	    event GamerIsLoaded Event_GamerIsLoaded;
	    //event NetworkClientEndWork EventNetworkClientEndWork;
	    event NetorkClientDisconnect EventNetorkClientDisconnect;
	}
}