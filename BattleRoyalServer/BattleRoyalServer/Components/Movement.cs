using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLibrary;
using CommonLibrary.CommonElements;
using CommonLibrary.GameMessages;
using Box2DX.Common;

namespace BattleRoyalServer
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

		public Movement(IGameObject parent, float speed) : base(parent)
		{
			this.speed = speed;
		}

		public override void Dispose()
		{
			body = null;
			Parent.Received_GoTo -= Handler_GoTo;
		}

		private void Handler_GoTo(IMessage msg)
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
					dY = speed;
					break;

				case DirectionVertical.Down:
					dY -= speed;
					break;
			}
			body.Body?.WakeUp();
			body.Body?.SetLinearVelocity(new Vec2(dX, dY));
		}

		public override void Setup()
		{
			this.body = Parent.Components?.GetComponent<SolidBody>();
			if (body == null)
			{
				Log.AddNewRecord("Ошибка создания компонента Movement", "Не получена сслыка на компонент SoldiBody");
				throw new Exception("Ошибка создания компонента Movement");
			}
			Parent.Received_GoTo += Handler_GoTo;
		}
	}
}