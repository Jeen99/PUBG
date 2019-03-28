using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSInteraction.Common;
using CSInteraction.ProgramMessage;
using Box2DX.Common;

namespace BattleRoayleServer
{
	public class Movement : Component, IMovement
	{
		private Direction currentDirection;
	
		/// <summary>
		/// Скорость игрока
		/// </summary>
		private float speed;
		/// <summary>
		/// Ссылка на тело перемещаемого игрока
		/// </summary>
		private ISolidBody body;

		public Movement(IGameObject parent, float speed) : base(parent)
		{
			this.speed = speed;
			this.body = parent?.Components?.GetComponent<SolidBody>();
			if (body == null)
			{
				Log.AddNewRecord("Ошибка создания компонента Movement", "Не получена сслыка на компонент SoldiBody");
				throw new Exception("Ошибка создания компонента Movement");
			}
		}

		public override void Dispose()
		{
			body = null;
		}

		public override void UpdateComponent(IMessage msg)
		{
			if (msg == null)
			{
				Log.AddNewRecord("Получено null сообщение в компоненте Movement");
				return;
			}

			switch (msg.TypeMessage)
				{
					case  TypesProgramMessage.GoTo:
						Handler_StartMoveGamer(msg as GoTo);
						break;
					case TypesProgramMessage.TimeQuantPassed:
						Handler_TimeQuantPassed();
						break;
				}
		
		}

		private void Handler_TimeQuantPassed()
		{

		}

		private void Handler_StartMoveGamer(GoTo msg)
		{
			currentDirection = msg.DirectionMove;
			float dX = 0;
			float dY = 0;

			// смещение по горизонтали
			switch (currentDirection.Horisontal)
			{
				case DirectionHorisontal.Left:
					dX -= speed;
					break;

				case DirectionHorisontal.Right:
					dX = speed;
					break;
			}
			// смещение по вертикали
			switch (currentDirection.Vertical)
			{
				case DirectionVertical.Up:
					dY -= speed;
					break;

				case DirectionVertical.Down:
					dY = speed;
					break;
			}
			body.Body?.SetLinearVelocity(new Vec2(dX, dY));
		}
	}
}