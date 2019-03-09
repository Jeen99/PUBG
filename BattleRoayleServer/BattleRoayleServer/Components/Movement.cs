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
		private bool active;
		/// <summary>
		/// Скорость игрока
		/// </summary>
		private double speed;
		/// <summary>
		/// Ссылка на тело перемещаемого игрока
		/// </summary>
		private SolidBody body;

		public Movement(GameObject parent) : base(parent)
		{

		}

		public override void Dispose()
		{
			throw new NotImplementedException();
		}

		public override void ProcessMsg(IComponentMsg msg)
		{
			throw new NotImplementedException();
		}
	}
}