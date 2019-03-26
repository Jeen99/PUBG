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

		public LootBox(IGameModel model, Collector collector, PointF location) : base(model)
		{
			this.components = new ConcurrentDictionary<Type, Component>();

			SolidBody body = new SolidBody(this, new System.Drawing.RectangleF(location, new SizeF(8, 8)),
				restetution, friction, density, TypesBody.Circle, TypesSolid.Transparent, (ushort)CollideCategory.Loot,
				(ushort)CollideCategory.Player);
			components.AddOrUpdate(body.GetType(), body, (k, v) => { return v; });
			collector.SetNewParent(this);
			components.AddOrUpdate(collector.GetType(), collector, (k, v) => { return v; });
		}

		public override TypesBehaveObjects TypesBehave { get; } = TypesBehaveObjects.Passive;

		public override TypesGameObject Type { get; } = TypesGameObject.LootBox;
	}
}
