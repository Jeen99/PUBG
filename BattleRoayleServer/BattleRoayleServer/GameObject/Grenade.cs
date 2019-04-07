using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Common;
using System.Drawing;
using Box2DX.Collision;
using Box2DX.Common;
using Box2DX.Dynamics;

namespace BattleRoayleServer
{
	public class Grenade : GameObject
	{
		private readonly float restetution = 0;
		private readonly float friction = 0;
		private readonly float density = 0.5f;
		private readonly float linearDamping = 0.85f;
		private readonly SizeF sizeGrenade = new SizeF(5,5);

		public Grenade(IModelForComponents model, PointF location, Vec2 startVelocity, IBullet grenadeBullet) : base(model)
		{
			#region CreateShape
			ShapeDef circleShape = new CircleDef();
			(circleShape as CircleDef).Radius = sizeGrenade.Width / 2;
			circleShape.Restitution = restetution;
			circleShape.Friction = friction;
			circleShape.Density = density;
			circleShape.Filter.CategoryBits = (ushort)CollideCategory.Grenade;
			circleShape.Filter.MaskBits = (ushort)CollideCategory.Box | (ushort)CollideCategory.Stone;

			ShapeDef sensorDef = new CircleDef();
			(sensorDef as CircleDef).Radius = grenadeBullet.Distance;
			sensorDef.Restitution = restetution;
			sensorDef.Friction = friction;
			sensorDef.Density = density;
			sensorDef.IsSensor = true;
			sensorDef.Filter.CategoryBits = (ushort)CollideCategory.Grenade;
			sensorDef.Filter.MaskBits = (ushort)CollideCategory.Player;

			#endregion

			var body = new SolidBody(this, new RectangleF(location, sizeGrenade), 
				new ShapeDef[] { circleShape , sensorDef }, linearDamping, startVelocity);
			Components.Add(body);

			var explosion = new Explosion(this, grenadeBullet);
			Components.Add(explosion);
		}

		public override TypesBehaveObjects TypesBehave { get; } = TypesBehaveObjects.Active;

		public override TypesGameObject Type { get; } = TypesGameObject.Grenade;


	}
}
