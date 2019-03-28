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

			body = new SolidBody(this, new RectangleF(location, new Size(10, 10)), restetution,
				friction, density, TypesBody.Circle, TypesSolid.Solid, (ushort)CollideCategory.Player,
				(ushort)CollideCategory.Box | (ushort)CollideCategory.Loot | (ushort)CollideCategory.Stone);
			Components.Add(body);

			var movement = new Movement(this, 40f);
			Components.Add(movement);

			var collector = new Collector(this);
			Components.Add(collector);

			var currentWeapon = new CurrentWeapon(this);
			Components.Add(currentWeapon);

			var healthy = new Healthy(this);
			Components.Add(healthy);

			var playerDied = new PlayerDied(this);
			Components.Add(playerDied);
		}

		/// <summary>
		/// Для упрощения доступа к расположения игрока на карте
		/// </summary>
		private ISolidBody body;

		public override TypesGameObject Type { get; } = TypesGameObject.Player;

		public override TypesBehaveObjects TypesBehave { get; } = TypesBehaveObjects.Active;

		public PointF Location
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