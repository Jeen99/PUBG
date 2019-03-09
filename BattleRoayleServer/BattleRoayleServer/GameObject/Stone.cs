using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoayleServer
{
	class Stone : GameObject
	{
		public override TypesGameObject Type { get; } = TypesGameObject.Stone;

        public override TypesBehaveObjects TypesBehave { get; } = TypesBehaveObjects.Passive;

        public Stone(IGameModel roomContext, Tuple<double, double> location, double radius)
		{
			Components = new List<Component>
			{
				new CircleBody(this,roomContext, location, radius, TypesSolid.Solid)
			};
		}
	}
}
