using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSInteraction.Common;
using System.Drawing;

namespace BattleRoayleServer
{
	

	public class Gun : Weapon
	{
		private const float restetution = 0;
		private const float friction = 0.3f;
		private const float density = 0;

		public Gun(PointF location, IGameModel context) : base(context)
		{
			this.Components = new List<Component>(1);
			SolidBody body = new SolidBody(this, new System.Drawing.RectangleF(location, new SizeF(8,8)),
				restetution, friction, density, TypesBody.Circle, TypesSolid.Transparent, (ushort)CollideCategory.Loot,
				(ushort)CollideCategory.Player);
			Components.Add(body);
		}

		public override TypesGameObject Type { get; } = TypesGameObject.Gun;

        public override TypesBehaveObjects TypesBehave { get;  } = TypesBehaveObjects.Passive;
    }
}