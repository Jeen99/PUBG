using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.ProgramMessage;

namespace BattleRoayleServer
{
	public class Explosion : Component
	{
		private IBullet grenadeBullet;
		private SolidBody bodyGrenade;
		private DateTime timeTillExplosion;

		public Explosion(IGameObject parent, IBullet grenadeBullet) : base(parent)
		{
			this.grenadeBullet = grenadeBullet;
			//добавил минуту, чтобы не было ошибки при вычитании
			timeTillExplosion = new DateTime(1, 1, 1, 0, 1, 4, 500);
		}

		public override void Setup()
		{
			this.bodyGrenade = Parent.Components?.GetComponent<SolidBody>();
			if (bodyGrenade == null)
			{
				Log.AddNewRecord("Ошибка создания компонента Explosion", "Не получена сслыка на компонент SolidBody");
				throw new Exception("Ошибка создания компонента Explosion");
			}
		}

		public override void UpdateComponent(IMessage msg)
		{
			switch (msg.TypeMessage)
			{
				case TypesProgramMessage.TimeQuantPassed:
					Handler_TimeQuantPassed(msg as TimeQuantPassed);
					break;
			}
		}

		private void Handler_TimeQuantPassed(TimeQuantPassed msg)
		{
			try
			{
				timeTillExplosion = timeTillExplosion.AddMilliseconds(-msg.QuantTime);
				if (timeTillExplosion.Second  == 59 )
				{
					MakeExplosion();
				}
			}
			catch (Exception e)
			{
				MakeExplosion();
			}
		}

		private void MakeExplosion()
		{
			//наносим урон все игрокам в зоне поражения
			foreach (var solidBody in bodyGrenade.CoveredObjects)
			{
				if (solidBody.Parent is Gamer)
				{
					solidBody.Parent.SendMessage(new GotDamage(grenadeBullet.Damage));
				}
			}
			//удаляем гранату
			Parent.Dispose();
		}
	}
}
