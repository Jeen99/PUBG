using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using CSInteraction.Common;
using System.Drawing;

namespace BattleRoayleServer
{
	public class LootBox : GameObject
	{

		private const float restetution = 0;
		private const float friction = 0.3f;
		private const float density = 0;

		public LootBox(IGameModel model, ICollector collector, PointF location) : base(model)
		{
			ISolidBody body = new SolidBody(this, new System.Drawing.RectangleF(location, new SizeF(8, 8)),
				restetution, friction, density, TypesBody.Circle, TypesSolid.Transparent, (ushort)CollideCategory.Loot,
				(ushort)CollideCategory.Player);
			Components.Add(body);
			collector.SetNewParent(this);
			Components.Add(collector);
		}

		public override TypesBehaveObjects TypesBehave { get; } = TypesBehaveObjects.Passive;

		public override TypesGameObject Type { get; } = TypesGameObject.LootBox;
	}
}
