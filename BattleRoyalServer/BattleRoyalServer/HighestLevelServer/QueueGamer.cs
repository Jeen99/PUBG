using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLibrary;
using CSInteraction.Server;

namespace BattleRoyalServer
{
    public class QueueGamer
    {
        public string NickName { get; private set; }

		public string Password { get; private set; }

		public ConnectedClient<IMessage> Client { get; private set; }

		/// <summary>
		/// Указывет, что данный игрок сейчас будет добавлне в комнату 
		/// и поэтому не может быть удален из очереди
		/// </summary>
		public bool AddInRoom { get; set;}

        public QueueGamer( ConnectedClient<IMessage> client, DataOfAccount data)
        {
            NickName = data.NickName;
			Password = data.Password;
            Client = client;
			AddInRoom = false;

		}
    }
}