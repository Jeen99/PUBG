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
       
        public ServerClient Client { get; private set; }

        public QueueGamer(string nickName, ServerClient client)
        {
            NickName = nickName;
            Client = client;
        }
    }
}