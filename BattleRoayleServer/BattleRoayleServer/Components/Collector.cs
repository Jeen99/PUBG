using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleRoayleServer
{
	public class Collector : Component
	{
		private BattleRoayleServer.Modifier[] modifiers;
		private Weapon[] weapons;
		/// <summary>
		/// Ссылка на тело перемещаемого игрока
		/// </summary>
		private SolidBody body;

		public Collector(IGameModel gameModel, GameObject parent) : base(gameModel, parent)
		{

		}
	}
}