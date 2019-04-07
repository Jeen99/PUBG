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
		private DateTime intervalBetweenDamage;

		public DamageZone(IGameObject parent) : base(parent)
		{
			intervalBetweenDamage = new DateTime(1, 1, 1, 0, 0, 0, durationIntervale);
		}

		public override void Setup()
		{
			bodyZone = Parent.Components.GetComponent<BodyZone>();
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
			intervalBetweenDamage = intervalBetweenDamage.AddMilliseconds(-msg.QuantTime);
			if(intervalBetweenDamage.Millisecond <= 0)
			{
				CreateDamage();
				intervalBetweenDamage = intervalBetweenDamage.AddMilliseconds(durationIntervale);
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
