using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using CommonLibrary;
using CommonLibrary.CommonElements;
using Box2DX.Collision;

namespace BattleRoayleServer
{
	public class Box:GameObject
	{
		private readonly float restetution = 0;
		private readonly float friction = 0;
		private readonly float density = 0;
		private readonly SizeF boxSize = new SizeF(15, 15);

		public Box(IModelForComponents context, PointF location) : base(context)
		{
			#region CreateShape
			ShapeDef RectangleDef = new PolygonDef();
			(RectangleDef as PolygonDef).SetAsBox(boxSize.Width / 2, boxSize.Height / 2);
			RectangleDef.Restitution = restetution;
			RectangleDef.Friction = friction;
			RectangleDef.Density = density;
			RectangleDef.Filter.CategoryBits = (ushort)CollideCategory.Box;
			RectangleDef.Filter.MaskBits = (ushort)CollideCategory.Player |
				(ushort)CollideCategory.Grenade | (ushort)CollideCategory.Loot;
			#endregion

			var body = new SolidBody(this, new RectangleF(location, boxSize), new ShapeDef[] { RectangleDef });
			Components.Add(body);
		}

		public override TypesBehaveObjects TypesBehave { get; } = TypesBehaveObjects.Passive;

		public override TypesGameObject Type { get;  } = TypesGameObject.Box;
	}


}
