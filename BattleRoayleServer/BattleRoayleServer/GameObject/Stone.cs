using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Common;
using System.Collections.Concurrent;
using Box2DX.Collision;

namespace BattleRoayleServer
{
	public class Stone : GameObject
	{
		private readonly float restetution = 0;
		private readonly float friction = 0.1f;
		private readonly float density = 0;

		public override TypesGameObject Type { get; } = TypesGameObject.Stone;

        public override TypesBehaveObjects TypesBehave { get; } = TypesBehaveObjects.Passive;

        public Stone(IModelForComponents roomContext, PointF location, Size size):base(roomContext)
		{
			#region CreateShape
			ShapeDef CircleDef = new CircleDef();
			(CircleDef as CircleDef).Radius = size.Width / 2;
			CircleDef.Restitution = restetution;
			CircleDef.Friction = friction;
			CircleDef.Density = density;
			CircleDef.Filter.CategoryBits = (ushort)CollideCategory.Stone;
			CircleDef.Filter.MaskBits = (ushort)CollideCategory.Player |
				(ushort)CollideCategory.Grenade | (ushort)CollideCategory.Loot;
			#endregion

			var body = new SolidBody(this, new RectangleF(location, size), new ShapeDef[] { CircleDef });
			Components.Add(body);
		}
	}
}
