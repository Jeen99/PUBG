using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSInteraction.ProgramMessage;
using CSInteraction.Common;

namespace BattleRoayleServer
{
	//отвечает за выстрел
	public class Shot:Component
	{
		/// <summary>
		/// Когда false - перезарядка, выстрел не возможен
		/// </summary>
		private bool reload;
		private Magazin magazin;

		public Shot(GameObject parent) : base(parent)
		{

		}

		public override void Dispose()
		{
			throw new NotImplementedException();
		}

		public override void UpdateComponent(IMessage msg)
		{
			throw new NotImplementedException();
		}
	}
}