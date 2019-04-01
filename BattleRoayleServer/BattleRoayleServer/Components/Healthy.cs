using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.ProgramMessage;

namespace BattleRoayleServer
{
	public class Healthy : Component, IHealthy
	{
		public Healthy(IGameObject parent) : base(parent)
		{
			HP = 100;
		}

		public float HP { get; private set; }

		public override void UpdateComponent(IMessage msg)
		{
			if (msg == null)
			{
				Log.AddNewRecord("Получено null сообщение в компоненте Healthy");
				return;
			}

			switch (msg.TypeMessage)
			{
				case TypesProgramMessage.GotDamage:
					Handler_GotDamage(msg as GotDamage);
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

		private void Handler_GotDamage(GotDamage msg)
		{
			HP -= msg.Damage;
			if (HP < 0)
			{
				Parent.Dispose();
			}
			Parent.Model?.AddEvent(new ChangedValueHP(Parent.ID, HP));
		}

		public override void Setup()
		{

		}
	}
}
