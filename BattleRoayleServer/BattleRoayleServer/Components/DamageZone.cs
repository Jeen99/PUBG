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
	public class DamageZone : Component
	{
		private BodyZone bodyZone;
		//в миллисекундах
		private const int durationIntervale = 500;
		private const float zoneIntervalDamage = 2;
		private TimeSpan intervalBetweenDamage;

		public DamageZone(IGameObject parent) : base(parent)
		{
			intervalBetweenDamage = new TimeSpan(0, 0, 0, 0, durationIntervale);
		}

		public override void Setup()
		{
			bodyZone = Parent.Components.GetComponent<BodyZone>();
			if (bodyZone == null)
			{
				Log.AddNewRecord("Ошибка создания компонента DamageZone", "Не получена сслыка на компонент BodyZone");
				throw new Exception("Ошибка создания компонента DamageZone");
			}
			Parent.Received_TimeQuantPassed += Handler_TimeQuantPassed;
		}

		private void Handler_TimeQuantPassed(IMessage msg)
		{
			//уменьшаем время до следующего получения урона от зоны	
			intervalBetweenDamage = intervalBetweenDamage.Add(new TimeSpan(0, 0, 0, 0, -msg.TimePassed));
			if (intervalBetweenDamage.Milliseconds < 0 && bodyZone.Create)
			{
				CreateDamage();
				intervalBetweenDamage = new TimeSpan(0, 0, 0, 0, durationIntervale);
			}	
		}

		private void CreateDamage()
		{
			//определяем игроков за краями зоны
			foreach(var player in Parent.Model.Players)
			{
				//определяем расстоняие до центра
				float distance = (float)Math.Sqrt(
					Math.Pow(player.Location.X - bodyZone.Location.X, 2) +
					Math.Pow(player.Location.Y - bodyZone.Location.Y, 2));
				if (bodyZone.Radius < distance)
				{
					//игрок получает урон
					(player as GameObject).Update(new GotDamage(this.Parent.ID, zoneIntervalDamage));
				}
			}
		}

		public override void Dispose()
		{
			Parent.Received_TimeQuantPassed -= Handler_TimeQuantPassed;
		}
	}
}
