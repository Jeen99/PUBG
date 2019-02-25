using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleRoayleServer
{
    public class RoyalRoom : IRoom
    {
        public INetwork NetworkLogic { get; private set; }

        public IRoomLogic GameLogic { get; private set; }

        public RoyalRoom(IList<QueueGamer> gamers)
        {
            //создаем игру на основе той информации, что пришла к нам с геймерами
        }
    }
}