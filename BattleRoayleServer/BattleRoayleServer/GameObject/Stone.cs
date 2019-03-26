using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Common;

namespace BattleRoayleServer
{
	class Stone : GameObject
	{
		public override TypesGameObject Type { get; } = TypesGameObject.Stone;

        public override TypesBehaveObjects TypesBehave { get; } = TypesBehaveObjects.Passive;

        public Stone(IGameModel roomContext, PointF location, Size size)
		{
			Components = new List<Component>(1);
			Components.Add(new SolidBody(this, roomContext, new RectangleF(location, size), TypesSolid.Solid));
		}
	}
}
