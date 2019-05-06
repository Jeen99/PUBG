using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;
using CommonLibrary.CommonElements;
using System.Collections.Concurrent;
using Box2DX.Collision;

namespace BattleRoayleServer
{
	public class Stone : GameObject
	{
		protected static PhysicsSetups physicsSetups = new PhysicsSetups(0, 0.1f, 0, 0);
		public static SizeF Size { get; protected set; } = new Size(16, 16);

		public override TypesGameObject Type { get; } = TypesGameObject.Stone;

        public override TypesBehaveObjects TypesBehave { get; } = TypesBehaveObjects.Passive;

        public Stone(IModelForComponents roomContext, PointF location):base(roomContext)
		{
			#region CreateShape
			ShapeDef CircleDef = new CircleDef();
			(CircleDef as CircleDef).Radius = Size.Width / 2;
			CircleDef.Restitution = physicsSetups.restetution;
			CircleDef.Friction = physicsSetups.friction;
			CircleDef.Density = physicsSetups.density;
			CircleDef.Filter.CategoryBits = (ushort)CollideCategory.Stone;
			CircleDef.Filter.MaskBits = (ushort)CollideCategory.Player |
				(ushort)CollideCategory.Grenade | (ushort)CollideCategory.Loot;
			#endregion

			var body = new SolidBody(this, new RectangleF(location, Size), new ShapeDef[] { CircleDef });
			Components.Add(body);
		}
	}
}
