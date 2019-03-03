using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoayleServer
{
	class CollisionObjects : IComponentMsg
	{
		public TypesComponentMsg Type { get; } = TypesComponentMsg.CollisionObjects;
		public IFieldObject Confront { get; private set; }

		public CollisionObjects(IFieldObject confront)
		{
			Confront = confront;
		}
	}
}
