using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CSInteraction.ProgramMessage;
using CSInteraction.Common;

namespace BattleRoayleServer
{
    public interface IGameModel
    {
        IList<IPlayer> Players { get;}
        IList<GameObject> GameObjects { get; }
        IField Field { get;}
		ObservableQueue<IMessage> HappenedEvents { get; }
		void RemoveGameObject(GameObject gameObject);

	}
}