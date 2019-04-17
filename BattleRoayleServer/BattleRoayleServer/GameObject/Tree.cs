using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Common;
using System.Drawing;
using Box2DX.Collision;

namespace BattleRoayleServer
{
	public class Tree : GameObject
	{
		private readonly float restetution = 0;
		private readonly float friction = 0;
		private readonly float density = 0;
		private readonly SizeF treerSize = new SizeF(15, 15);

		public Tree(IModelForComponents model, PointF location) : base(model)
		{
			#region CreateShape
			ShapeDef RectangleDef = new PolygonDef();
			(RectangleDef as PolygonDef).SetAsBox(treerSize.Width / 2, treerSize.Height / 2);
			RectangleDef.Restitution = restetution;
			RectangleDef.Friction = friction;
			RectangleDef.Density = density;
			RectangleDef.Filter.CategoryBits = (ushort)CollideCategory.Box;
			RectangleDef.Filter.MaskBits = (ushort)CollideCategory.Player |
				(ushort)CollideCategory.Grenade | (ushort)CollideCategory.Loot;
			#endregion

			var body = new SolidBody(this, new RectangleF(location, treerSize), new ShapeDef[] { RectangleDef });
			Components.Add(body);
		}

		public override TypesBehaveObjects TypesBehave { get; } = TypesBehaveObjects.Passive;

		public override TypesGameObject Type { get; } = TypesGameObject.Tree;
	}
}
