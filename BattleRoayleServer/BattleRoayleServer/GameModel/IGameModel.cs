using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace BattleRoayleServer
{
    public interface IGameModel
    {
        IList<IPlayer> Players { get;}
        IList<GameObject> GameObjects { get; }
        IField Field { get;}
		ObservableCollection<IComponentEvent> HappenedEvents { get; }
		void RemoveGameObject(GameObject gameObject);

	}
}