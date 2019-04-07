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
		private readonly SizeF sizeLootBox = new SizeF(8, 8);

		public LootBox(IModelForComponents model, ICollector collector, PointF location) : base(model)
		{
			var body = new TransparentBody(this, new System.Drawing.RectangleF(location, sizeLootBox));
			Components.Add(body);
			collector.SetNewParent(this);
			Components.Add(collector);

			model.AddLoot(this);
		}

		public override TypesBehaveObjects TypesBehave { get; } = TypesBehaveObjects.Passive;

		public override TypesGameObject Type { get; } = TypesGameObject.LootBox;
	}
}
