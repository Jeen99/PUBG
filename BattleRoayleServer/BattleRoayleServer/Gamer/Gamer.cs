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
		public Gamer(PointF location, IGameModel context):base()
		{
			this.Components = new List<Component>
			{
				new CircleBody(this, context, location, 1.5F, TypesSolid.Solid),			
			};
			Components.Add(new Movement(this, (SolidBody)Components[0], 2));
			body = (SolidBody)Components[0];
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
				return body.Location;
			}
		}

		

		public void PerformAction(IMessage action)
		{
			switch (action.TypeMessage)
			{
				case TypesProgramMessage.GoTo:
					Handler_GoTo((GoTo)action);
					break;
				case TypesProgramMessage.StopMove:
					Handler_StopMove((StopMove)action);
					break;
				default:
					//записываем в лог, сообщение что не смогли обработать сообщение
					Handler_StandartExceptions.Handler_ErrorHandlingClientMsg(this.ToString(), action.TypeMessage.ToString());
					break;
			}
		}

		private void Handler_StopMove(StopMove msg)
		{
			SendMessage(new EndMoveGamer());
		}

		private void Handler_GoTo(GoTo msg)
		{
			SendMessage(new StartMoveGamer(msg.DirectionMove));
		}
	}
}