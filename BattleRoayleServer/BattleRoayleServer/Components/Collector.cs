using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleRoayleServer
{
	public class Collector : Component
	{
		private Modifier[] modifiers;
		private Weapon[] weapons;
		/// <summary>
		/// Ссылка на тело перемещаемого игрока
		/// </summary>
		private SolidBody body;

		public Collector(GameObject parent) : base(parent)
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