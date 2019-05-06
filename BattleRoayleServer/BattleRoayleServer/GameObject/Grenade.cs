using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;
using CommonLibrary.CommonElements;
using System.Drawing;
using Box2DX.Collision;
using Box2DX.Common;
using Box2DX.Dynamics;

namespace BattleRoayleServer
{
	public class Grenade : GameObject
	{
		protected static PhysicsSetups physicsSetups = new PhysicsSetups(0, 0, 0.5f, 0.85f);
		public static SizeF Size { get; protected set; } = new SizeF(5,5);

		public Grenade(IModelForComponents model, PointF location, Vec2 startVelocity, IBullet grenadeBullet) : base(model)
		{
			#region CreateShape
			ShapeDef circleShape = new CircleDef();
			(circleShape as CircleDef).Radius = Size.Width / 2;
			circleShape.Restitution = physicsSetups.restetution;
			circleShape.Friction = physicsSetups.friction;
			circleShape.Density = physicsSetups.density;
			circleShape.Filter.CategoryBits = (ushort)CollideCategory.Grenade;
			circleShape.Filter.MaskBits = (ushort)CollideCategory.Box | (ushort)CollideCategory.Stone;

			ShapeDef sensorDef = new CircleDef();
			(sensorDef as CircleDef).Radius = grenadeBullet.Distance;
			sensorDef.Restitution = physicsSetups.restetution;
			sensorDef.Friction = physicsSetups.friction;
			sensorDef.Density = physicsSetups.density;
			sensorDef.IsSensor = true;
			sensorDef.Filter.CategoryBits = (ushort)CollideCategory.Grenade;
			sensorDef.Filter.MaskBits = (ushort)CollideCategory.Player;
			#endregion

			var body = new SolidBody(this, new RectangleF(location, Size), 
				new ShapeDef[] { circleShape , sensorDef }, physicsSetups.linearDamping, startVelocity);
			Components.Add(body);

			var explosion = new Explosion(this, grenadeBullet);
			Components.Add(explosion);
		}

		public override TypesBehaveObjects TypesBehave { get; } = TypesBehaveObjects.Active;

		public override TypesGameObject Type { get; } = TypesGameObject.Grenade;


	}
}
