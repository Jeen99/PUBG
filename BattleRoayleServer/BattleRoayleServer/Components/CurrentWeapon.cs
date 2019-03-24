using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.ProgramMessage;

namespace BattleRoayleServer
{
	public class CurrentWeapon : Component
	{
		private Collector inventory;
		private Weapon currentWeapon;
		public CurrentWeapon(GameObject parent, Collector collector) : base(parent)
		{
			inventory = collector;
			currentWeapon = null;
		}

		public override void Dispose()
		{
			throw new NotImplementedException();
		}

		public override void ProcessMsg(IMessage msg)
		{
			throw new NotImplementedException();
		}
	}
}
