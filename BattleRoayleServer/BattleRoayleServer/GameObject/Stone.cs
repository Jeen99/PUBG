﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Common;
using System.Collections.Concurrent;

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
			var body = new SolidBody(this, new RectangleF(location, size), restetution,
				friction, density, TypesBody.Circle, (ushort)CollideCategory.Stone,
				(ushort)CollideCategory.Player);
			Components.Add(body);
		}
	}
}
