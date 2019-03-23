using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSInteraction.Common;
using Box2DX.Common;

namespace BattleRoayleServer
{
	public class Movement : Component
	{
		private Direction currentDirection;
	
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
					case TypesComponentMsg.TimeQuantPassed:
						Handler_TimeQuantPassed();
						break;
				}
			}

		}

		private void Handler_TimeQuantPassed()
		{

		}

		private void Handler_StartMoveGamer(StartMoveGamer msg)
		{
			currentDirection = msg.Direction;
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
			body.Body.SetLinearVelocity(new Vec2(dX, dY));
		}
	}
}