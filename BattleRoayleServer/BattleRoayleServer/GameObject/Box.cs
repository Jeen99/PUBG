using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Common;
using CSInteraction.ProgramMessage;

namespace BattleRoayleServer
{
	 class Box:GameObject
	{
		public Box(IGameModel context, Tuple<double, double> location): base()
		{
			this.Components = new List<Component>
			{
				new RectangleBody(this, context, location, new Tuple<double, double>(3,3), TypesSolid.Solid)
			};
		}

		public override TypesBehaveObjects TypesBehave { get; } = TypesBehaveObjects.Passive;

		public override TypesGameObject Type { get;  } = TypesGameObject.Box;
	}


}
