using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSInteraction.Common;

namespace BattleRoayleServer
{
	public class Movement : Component
	{
		private Direction currentDirection;
		/// <summary>
		/// Если true игрок двигается в настоящее время
		/// </summary>
		private bool active;
		/// <summary>
		/// Скорость игрока
		/// </summary>
		private float speed;
		/// <summary>
		/// Ссылка на тело перемещаемого игрока
		/// </summary>
		private SolidBody body;

		public Movement(GameObject parent, SolidBody body, float speed) : base(parent)
		{
			this.speed = speed;
			this.body = body;
		}

		public override void Dispose()
		{
			return;
		}

		public override void ProcessMsg(IComponentMsg msg)
		{
			if (msg != null)
			{
				switch (msg.Type)
				{
					case TypesComponentMsg.StartMoveGamer:
						Handler_StartMoveGamer(msg as StartMoveGamer);
						break;
					case TypesComponentMsg.EndMoveGamer:
						Handler_EndMoveGamer();
						break;
					case TypesComponentMsg.TimeQuantPassed:
						Handler_TimeQuantPassed(msg as TimeQuantPassed);
						break;
						/*case TypesComponentMsg.CollisionObjects:
							Handler_CollisionObjects((CollisionObjects)msg);
							break;*/
				}
			}

		}

		/*private void Handler_CollisionObjects(CollisionObjects msg)
		{
		
		}*/

		private void Handler_EndMoveGamer()
		{
			//прекращаем движение
			active = false;
		}

		private void Handler_TimeQuantPassed(TimeQuantPassed msg)
		{
			if (active)
			{
				// если не задано направление, то останавливаем движение
				if (currentDirection.Horisontal == DirectionHorisontal.None &&
					currentDirection.Vertical == DirectionVertical.None)
				{
					active = false;
					return;
				}

				float dX = 0;
				float dY = 0;
				float delta = (float)(speed * msg.QuantTime / 1000);

				// смещение по горизонтали
				switch(currentDirection.Horisontal)
				{
					case DirectionHorisontal.Left:
						dX -= delta;
						break;

					case DirectionHorisontal.Right:
						dX = delta;
						break;
				}
				// смещение по вертикали
				switch (currentDirection.Vertical)
				{
					case DirectionVertical.Up:
						dY -= delta;
						break;

					case DirectionVertical.Down:
						dY = delta;
						break;
				}

				body.AppendCoords(dX, dY);	
			}
		}

		private void Handler_StartMoveGamer(StartMoveGamer msg)
		{
			currentDirection = msg.Direction;
			active = true;
		}
	}
}