using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using CSInteraction.ProgramMessage;
using CSInteraction.Common;

namespace BattleRoayleServer
{
	public class Gamer : GameObject, IPlayer
	{
		private const float restetution = 0.2f;
		private const float friction = 0.3f;
		private const float density = 0.5f;

		public Gamer(PointF location, IGameModel context) : base(context)
		{
			this.Components = new List<Component>(3);
			body = new SolidBody(this, new RectangleF(location, new Size(10, 10)), restetution,
				friction, density, TypesBody.Circle, TypesSolid.Solid, (ushort)CollideCategory.Player,
				(ushort)CollideCategory.Box | (ushort)CollideCategory.Loot | (ushort)CollideCategory.Stone);
			Components.Add(body);
			Components.Add(new Movement(this, body, 40f));
			Components.Add(new Collector(this, body));

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
			switch (action.TypeMessage)
			{
				case TypesProgramMessage.GoTo:
					Handler_GoTo((GoTo)action);
					break;
				case TypesProgramMessage.TryPickUp:
					Handler_TryPickUp();
					break;
				case TypesProgramMessage.MakeReloadWeapon:
					Handler_MakeReloadWeapon();
						break;
				default:
					//записываем в лог, сообщение что не смогли обработать сообщение
					Handler_StandartExceptions.Handler_ErrorHandlingClientMsg(this.ToString(), action.TypeMessage.ToString());
					break;
			}
		}

		private void Handler_TryPickUp()
		{
			SendMessage(new PickUpLoot());
		}

		private void Handler_GoTo(GoTo msg)
		{
			SendMessage(new StartMoveGamer(msg.DirectionMove));
		}

		private void Handler_MakeReloadWeapon()
		{
			SendMessage(new MakeReload());
		}
	}
}