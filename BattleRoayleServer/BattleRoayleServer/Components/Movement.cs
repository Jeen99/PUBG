using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSInteraction.Common;
using CSInteraction.ProgramMessage;
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
			body = null;
			return;
		}

		public override void UpdateComponent(IMessage msg)
		{
			if (msg != null)
			{
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
			body.Body.SetLinearVelocity(new Vec2(dX, dY));
		}
	}
}