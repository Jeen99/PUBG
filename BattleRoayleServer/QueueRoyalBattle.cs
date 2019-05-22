using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSInteraction.Server;

namespace BattleRoayleServer
{
    class QueueRoyalBattle
    {
        private List<QueueGamer> queueOfGamer = new List<QueueGamer>();

        public QueueRoyalBattle()
        {
            queueOfGamer = new List<QueueGamer>();
        }

        public void AddGamer(QueueGamer gamer)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteOfQueue(ServerClient client)
        {
            throw new System.NotImplementedException();
        }
    }
}