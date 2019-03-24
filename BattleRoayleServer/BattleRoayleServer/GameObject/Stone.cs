using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Common;
using System.Collections.Concurrent;

namespace BattleRoayleServer
{
	class Stone : GameObject
	{
		private const float restetution = 0.2f;
		private const float friction = 0.1f;
		private const float density = 0;

		public override TypesGameObject Type { get; } = TypesGameObject.Stone;

        public override TypesBehaveObjects TypesBehave { get; } = TypesBehaveObjects.Passive;

        public Stone(IGameModel roomContext, PointF location, Size size):base(roomContext)
		{
			components = new ConcurrentDictionary<Type, Component>();
			var body = new SolidBody(this, new RectangleF(location, size), restetution,
				friction, density, TypesBody.Circle, TypesSolid.Solid, (ushort)CollideCategory.Box,
				(ushort)CollideCategory.Player);
			components.AddOrUpdate(body.GetType(), body, (k, v) => { return v; });
		}
	}
}
