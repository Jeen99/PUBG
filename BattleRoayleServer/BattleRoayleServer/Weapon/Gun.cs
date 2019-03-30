using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSInteraction.Common;
using System.Drawing;
using System.Collections.Concurrent;

namespace BattleRoayleServer
{
	
	public class Gun : Weapon
	{
		private const float restetution = 0;
		private const float friction = 0.3f;
		private const float density = 0;

		public Gun( IModelForComponents context, PointF location) : base(context)
		{
			var body = new TransparentBody(this, new System.Drawing.RectangleF(location, new SizeF(8,8)));
			Components.Add(body);

			var magazin = new Magazin(this, TypesWeapon.Gun, 500, 3000);
			Components.Add(magazin);

			var shot = new Shot(this);
			Components.Add(shot);

			TypeWeapon = TypesWeapon.Gun;

			context.AddLoot(this);
		}
		
		public override TypesGameObject Type { get; } = TypesGameObject.Weapon;

        public override TypesBehaveObjects TypesBehave { get;  } = TypesBehaveObjects.Passive;

	}
}