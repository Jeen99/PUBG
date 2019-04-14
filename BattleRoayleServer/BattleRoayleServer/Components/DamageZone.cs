using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.ProgramMessage;

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
		}

		public override void UpdateComponent(IMessage msg)
		{
			switch (msg.TypeMessage)
			{
				case TypesProgramMessage.TimeQuantPassed:
					Handler_TimeQuantPassed((TimeQuantPassed)msg);
					break;
			}
		}

		private void Handler_TimeQuantPassed(TimeQuantPassed msg)
		{
			//уменьшаем время до следующего получения урона от зоны	
			intervalBetweenDamage = intervalBetweenDamage.Add(new TimeSpan(0, 0, 0, 0, -msg.QuantTime));
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
					Math.Pow(player.Location.X - bodyZone.Location.X, 2));
				if (bodyZone.Radius < distance)
				{
					//игрок получает урон
					player.SendMessage(new GotDamage(zoneIntervalDamage));
				}
			}
		}
	}
}
