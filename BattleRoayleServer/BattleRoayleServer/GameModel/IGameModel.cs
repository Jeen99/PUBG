using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CSInteraction.ProgramMessage;
using CSInteraction.Common;
using System.Collections.Concurrent;

namespace BattleRoayleServer
{
    public interface IGameModel
    {
        IList<IPlayer> Players { get;}
		ConcurrentDictionary<ulong, GameObject> GameObjects { get; }
        IField Field { get;}
		ObservableQueue<IMessage> HappenedEvents { get; }
		void RemoveGameObject(GameObject gameObject);

	}
}