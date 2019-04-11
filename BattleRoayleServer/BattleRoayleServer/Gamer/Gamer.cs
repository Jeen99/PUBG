using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using CSInteraction.ProgramMessage;
using CSInteraction.Common;
using Box2DX.Collision;

namespace BattleRoayleServer
{
	public class Gamer : GameObject, IPlayer
	{
		private readonly float restetution = 0;
		private readonly float friction = 0;
		private readonly float density = 0.5f;
		private readonly SizeF GamerSize = new SizeF(10,10);

		public event PlayerDeleted EventPlayerDeleted;

		public Gamer(IModelForComponents context, PointF location) : base(context)
		{
			#region CreateShape
			ShapeDef CircleDef = new CircleDef();
			(CircleDef as CircleDef).Radius = GamerSize.Width / 2;
			CircleDef.Restitution = restetution;
			CircleDef.Friction = friction;
			CircleDef.Density = density;
			CircleDef.Filter.CategoryBits = (ushort)CollideCategory.Player;
			CircleDef.Filter.MaskBits = (ushort)CollideCategory.Box | (ushort)CollideCategory.Stone 
				| (ushort)CollideCategory.Grenade;
			#endregion

			body = new SolidBody(this, new RectangleF(location, GamerSize), new ShapeDef[] { CircleDef });
			Components.Add(body);

			var movement = new Movement(this, 40f);
			Components.Add(movement);

			var collector = new Collector(this);
			Components.Add(collector);

			var currentWeapon = new CurrentWeapon(this);
			Components.Add(currentWeapon);

			var healthy = new Healthy(this);
			Components.Add(healthy);

			var statistics = new Statistics(this);
			Components.Add(statistics);
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

		public override void Dispose()
		{
			Model.AddEvent(new EndGame(ID));
			base.Dispose();
			EventPlayerDeleted?.Invoke(this);
		}
	}
}