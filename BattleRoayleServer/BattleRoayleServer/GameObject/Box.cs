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
		protected static PhysicsSetups physicsSetups = new PhysicsSetups(0, 0, 0, 0);
		public static SizeF Size { get; protected set; } = new SizeF(15, 15);

		public Box(IModelForComponents context, PointF location) : base(context)
		{
			#region CreateShape
			ShapeDef RectangleDef = new PolygonDef();
			(RectangleDef as PolygonDef).SetAsBox(Size.Width / 2, Size.Height / 2);
			RectangleDef.Restitution = physicsSetups.restetution;
			RectangleDef.Friction = physicsSetups.friction;
			RectangleDef.Density = physicsSetups.density;
			RectangleDef.Filter.CategoryBits = (ushort)CollideCategory.Box;
			RectangleDef.Filter.MaskBits = (ushort)CollideCategory.Player |
				(ushort)CollideCategory.Grenade | (ushort)CollideCategory.Loot;
			#endregion

			var body = new SolidBody(this, new RectangleF(location, Size), new ShapeDef[] { RectangleDef });
			Components.Add(body);
		}

		public override TypesBehaveObjects TypesBehave { get; } = TypesBehaveObjects.Passive;

		public override TypesGameObject Type { get;  } = TypesGameObject.Box;
	}


}
