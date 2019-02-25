using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleRoayleServer
{
    public interface IField
    {
        void Put(string gameobject);
        void Remove(GameObject gameObject);
    }
}