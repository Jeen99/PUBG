using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSInteraction.Common;

namespace BattleRoayleServer
{
	public class Movement:Component
	{
		private Directions currentDirection;
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
				float dX = 0;
				float dY = 0;
				float delta = (float)(speed * msg.QuantTime / 1000);

				switch (currentDirection)
				{
					case Directions.bottom:
						dY -= delta;
						break;
					case Directions.left:
						dX -= delta;
						break;
					case Directions.left_bottom:
						dX -= delta;
						dY -= delta;
						break;
					case Directions.left_top:
						dX -= delta;
						dY = delta;
						break;
					case Directions.right:
						dX = delta;
						break;
					case Directions.right_bottom:
						dX = delta;
						dY -= delta;
						break;
					case Directions.right_top:
						dX = delta;
						dY = delta;
						break;
					case Directions.top:
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