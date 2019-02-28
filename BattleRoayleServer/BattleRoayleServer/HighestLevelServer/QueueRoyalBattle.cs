using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSInteraction.Server;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace BattleRoayleServer
{
    class QueueRoyalBattle
    {
        private ObservableCollection<QueueGamer> queueOfGamer;

        public QueueRoyalBattle()
        {
			queueOfGamer = new ObservableCollection<QueueGamer>();
		}

        public void AddGamer(QueueGamer gamer)
        {
			queueOfGamer.Add(gamer);
        }

        public void DeleteOfQueue(QueueGamer gamer)
        {
            throw new System.NotImplementedException();
        }
    }
}