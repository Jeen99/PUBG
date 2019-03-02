using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleRoayleServer
{
    public interface IField
    {
        void Put(GameObject gameobject);
        void Remove(GameObject gameObject);
    }
}