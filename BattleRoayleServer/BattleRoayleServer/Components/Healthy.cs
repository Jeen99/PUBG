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
		public Healthy(IGameObject parent) : base(parent)
		{
			HP = 100;
		}

		public float HP { get; private set; }

		public override void UpdateComponent(IMessage msg)
		{
			switch (msg.TypeMessage)
			{
				case TypesProgramMessage.GotDamage:
					Handler_UpdateComponent(msg as GotDamage);
					break;
			}
		}
		public override IMessage State
		{
			get
			{
				return new HealthyState(HP);
			}
		}
		private void Handler_UpdateComponent(GotDamage msg)
		{
			HP -= msg.Damage;
			if (HP < 0)
			{
				Parent.Dispose();
			}
			Parent.Model.HappenedEvents.Enqueue(new ChangedValueHP(HP));
		}
	}
}
