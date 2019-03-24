using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.ProgramMessage;

namespace BattleRoayleServer
{
	public class Healthy : Component
	{
		public Healthy(GameObject parent) : base(parent)
		{
			HP = 100;
		}

		public float HP { get; private set; }

		public override void Dispose()
		{
			throw new NotImplementedException();
		}

		public override void UpdateComponent(IMessage msg)
		{
			switch (msg.TypeMessage)
			{
				case TypesProgramMessage.GotDamage:
					Handler_UpdateComponent(msg as GotDamage);
					break;
			}
		}

		private void Handler_UpdateComponent(GotDamage msg)
		{
			HP -= msg.Damage;
			if (HP < 0)
			{
				Parent.SendMessage(new HappenedDeath());
			}
			Parent.Model.HappenedEvents.Enqueue(new ChangedValueHP(HP));
		}
	}
}
