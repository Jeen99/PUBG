using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;
using CommonLibrary.CommonElements;
using CommonLibrary.GameMessages;

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
			if (msg == null)
			{
				Log.AddNewRecord("Получено null сообщение в компоненте Healthy");
				return;
			}

			switch (msg.TypeMessage)
			{
				case TypesMessage.GotDamage:
					Handler_GotDamage(msg);
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

		private void Handler_GotDamage(IMessage msg)
		{
			HP -= msg.Damage;
			if (HP <= 0)
			{
				Parent.Model?.AddOutgoingMessage(new ChangedValueHP(Parent.ID, HP));
				Parent.Update( new GamerDied(this.Parent.ID));
				Parent.SetDestroyed();
			}
			else
			{
				Parent.Model?.AddOutgoingMessage(new ChangedValueHP(Parent.ID, HP));
			}
		}

		public override void Setup()
		{

		}
	}
}
