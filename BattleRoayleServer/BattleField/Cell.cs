using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BattleRoayleServer
{
    abstract class Cell
    {
        public Point Position { get; private set;}
        protected List<IPlaced> placedThings;
    }
}
