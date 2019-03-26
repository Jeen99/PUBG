using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using CSInteraction.ProgramMessage;
using CSInteraction.Common;

namespace BattleRoayleServer
{
	public class Gamer : GameObject, IPlayer
	{
		private const float restetution = 0.2f;
		private const float friction = 0.3f;
		private const float density = 0.5f;

		public event PlayerDeleted EventPlayerDeleted;

		public Gamer(PointF location, IGameModel context) : base(context)
		{

			this.components = new ConcurrentDictionary<Type, Component>();
			body = new SolidBody(this, new RectangleF(location, new Size(10, 10)), restetution,
				friction, density, TypesBody.Circle, TypesSolid.Solid, (ushort)CollideCategory.Player,
				(ushort)CollideCategory.Box | (ushort)CollideCategory.Loot | (ushort)CollideCategory.Stone);
			components.AddOrUpdate(body.GetType(), body, (k, v) => { return v; });

			var movement = new Movement(this, body, 40f);
			components.AddOrUpdate(movement.GetType(), movement, (k, v) => { return v; });

			var collector = new Collector(this, body);
			components.AddOrUpdate(collector.GetType(), collector, (k, v) => { return v; });

			var currentWeapon = new CurrentWeapon(this, collector);
			components.AddOrUpdate(currentWeapon.GetType(), currentWeapon, (k, v) => { return v; });

			var healthy = new Healthy(this);
			components.AddOrUpdate(healthy.GetType(), healthy, (k, v) => { return v; });

			var playerDied = new PlayerDied(this);
			components.AddOrUpdate(playerDied.GetType(), playerDied, (k, v) => { return v; });
		}

		/// <summary>
		/// Для упрощения доступа к расположения игрока на карте
		/// </summary>
		private SolidBody body;

		public override TypesGameObject Type { get; } = TypesGameObject.Player;

		public override TypesBehaveObjects TypesBehave { get; } = TypesBehaveObjects.Active;


		PointF IPlayer.Location
		{
			get
			{
				return body.Shape.Location;
			}
		}

		public void PerformAction(IMessage action)
		{
			SendMessage(action);
		}

		public override void Dispose()
		{
			base.Dispose();
			EventPlayerDeleted?.Invoke(this);
		}
	}
}