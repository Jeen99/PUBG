using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CSInteraction.ProgramMessage;
using CSInteraction.Common;
using System.Collections.Concurrent;
using Box2DX.Common;
using Box2DX.Collision;
using Box2DX.Dynamics;

namespace BattleRoayleServer
{
    public interface IGameModel
    {
        IList<IPlayer> Players { get;}
		Dictionary<ulong, GameObject> GameObjects { get; }
        World Field { get;}
		ObservableQueue<IMessage> HappenedEvents { get; }
		void AddGameObject(GameObject gameObject);
		void RemoveGameObject(GameObject gameObject);
		void Dispose();
	}
}