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
        string Nick { get; }

		RectangleF VisibleArea { get; }
		string Password { get; }
		event GamerIsLoaded Event_GamerIsLoaded;
		//event NetworkClientEndWork EventNetworkClientEndWork;
		event NetorkClientDisconnect EventNetorkClientDisconnect;

		void SendMessgaeToClient(IMessage msg);
		void SaveStatistics(IMessage msg);
		void Dispose();
	    IPlayer Player { get; }
	}
}