using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleRoayleServer
{
    public interface IField
    {
        void Put(IFieldObject fieldObject);
        void Remove(IFieldObject fieldObject);
		void Move(IFieldObject fieldObject);
    }
}