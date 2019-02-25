using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleRoayleServer
{
    public interface IGameObjectEvent
	{
		TypesGameObject TypeObject { get; }
		IList<IComponentEvent> ComponentsEvents { get;  }
	}
}