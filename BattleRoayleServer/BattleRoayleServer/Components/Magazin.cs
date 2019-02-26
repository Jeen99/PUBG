using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleRoayleServer
{
	public class Magazin:Component
	{
		/// <summary>
		/// Когда true - осуществляется перезарядка магазина
		/// </summary>
		private bool reload;

		public Magazin(IGameModel gameModel, GameObject parent) : base(gameModel, parent)
		{

		}
	}
}