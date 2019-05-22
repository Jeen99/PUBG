using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BattleRoayleServer
{ 
    interface IPlaced
    {
       Point Location { get; set; }
       TypesObject GetTypesObject();
    }

    enum TypesObject
    {
        Weapon,
        Equipment,
        Gamer
    }
}
