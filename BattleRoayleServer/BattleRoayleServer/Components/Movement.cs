using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
	}
}