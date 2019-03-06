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

		public Magazin(GameObject parent) : base(parent)
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