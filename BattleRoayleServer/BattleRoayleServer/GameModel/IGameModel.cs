using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleRoayleServer
{
    public interface IGameModel
    {
        IList<IPlayer> Players { get;}
        System.Collections.Generic.IList<GameObject> GameObjects { get; }
        IField Field { get;}
    }
}