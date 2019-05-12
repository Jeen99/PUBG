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
	public class Explosion : Component
	{
		private IBullet grenadeBullet;
		private SolidBody bodyGrenade;
		private TimeSpan timeTillExplosion;

		public Explosion(IGameObject parent, IBullet grenadeBullet) : base(parent)
		{
			this.grenadeBullet = grenadeBullet;
			timeTillExplosion = new TimeSpan(0, 0, 0, 4, 500);
		}

		public override void Setup()
		{
			this.bodyGrenade = Parent.Components?.GetComponent<SolidBody>();
			if (bodyGrenade == null)
			{
				Log.AddNewRecord("Ошибка создания компонента Explosion", "Не получена сслыка на компонент SolidBody");
				throw new Exception("Ошибка создания компонента Explosion");
			}
			Parent.Received_TimeQuantPassed += Handler_TimeQuantPassed;
		}

		private void Handler_TimeQuantPassed(IMessage msg)
		{
			timeTillExplosion = timeTillExplosion.Add(new TimeSpan(0, 0, 0, 0, -msg.TimePassed));
			if(timeTillExplosion.TotalMilliseconds <= 0)
				MakeExplosion();
		}

		private void MakeExplosion()
		{
			//наносим урон все игрокам в зоне поражения
			foreach (var solidBody in bodyGrenade.CoveredObjects)
			{
				if (solidBody.Parent is Gamer)
				{
					solidBody.Parent.Update(new GotDamage(this.Parent.ID, grenadeBullet.Damage));
				}
			}
			//удаляем гранату
			Parent.SetDestroyed();
		}
		public override void Dispose()
		{
			Parent.Received_TimeQuantPassed -= Handler_TimeQuantPassed;
		}
	}
}
