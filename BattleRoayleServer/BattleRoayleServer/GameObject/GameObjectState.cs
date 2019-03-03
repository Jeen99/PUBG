using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleRoayleServer
{
    public class GameObjectState
    {
		public ulong ID { get; private set; }
		public TypesGameObject Type { get; private set; }
		public IList<ComponentState> ComponentStates { get; private set; }

		public GameObjectState(ulong iD, TypesGameObject type, IList<ComponentState> componentStates)
		{
			ID = iD;
			Type = type;
			ComponentStates = componentStates;
		}
	}
}