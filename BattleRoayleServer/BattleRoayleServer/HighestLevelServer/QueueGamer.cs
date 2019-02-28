using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSInteraction.Server;

namespace BattleRoayleServer
{
    public class QueueGamer
    {
        public string NickName { get; private set; }

		public string Password { get; private set; }

		public ServerClient Client { get; private set; }

		/// <summary>
		/// Указывет, что данный игрок сейчас будет добавлне в комнату 
		/// и поэтому не может быть удален из очереди
		/// </summary>
		public bool AddInRoom { get; set;}

        public QueueGamer( ServerClient client, DataOfAccount data)
        {
            NickName = data.NickName;
			Password = data.Password;
            Client = client;
			AddInRoom = false;

		}
    }
}