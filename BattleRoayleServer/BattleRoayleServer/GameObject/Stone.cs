using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoayleServer
{
	class Stone : GameObject
	{
		public override TypesGameObject Type { get; } = TypesGameObject.Stone;

        public override TypesBehaveObjects TypesBehave => throw new NotImplementedException();

        public Stone(IGameModel roomContext, PointF location, float radius, byte angle)
		{
			Components = new List<Component>
			{
				new CircleBody(this,roomContext, location, radius, TypesSolid.Solid)
			};
		}
	}
}
