using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.ProgramMessage;

namespace BattleRoayleServer
{
	public class Statistics : Component
	{
		private int kills = 0;
		private TimeSpan timeInBattle = new TimeSpan();
		private bool playerDied = false;

		public Statistics(IGameObject parent) : base(parent)
		{

		}

		public override void UpdateComponent(IMessage msg)
		{
			switch (msg.TypeMessage)
			{
				case TypesProgramMessage.TimeQuantPassed:
					Handler_TimeQuantPassed(msg as TimeQuantPassed);
					break;
				case TypesProgramMessage.MakedKill:
					Handler_MakedKill();
					break;
				case TypesProgramMessage.PlayerDied:
					Handler_PlayerDied();
					break;
			}
		}

		private void Handler_PlayerDied()
		{
			playerDied = true;
		}

		private void Handler_MakedKill()
		{
			kills++;
		}

		private void Handler_TimeQuantPassed(TimeQuantPassed msg)
		{
			timeInBattle = timeInBattle.Add(new TimeSpan(0, 0, 0, 0, msg.QuantTime));
		}

		//выполняет некоторые действия при смерти игрока
		public override void Dispose()
		{
			//сохраняем статистику игрока
			Parent.Model.AddEvent(new PlayerBattleStatistics(Parent.ID, playerDied, kills, timeInBattle));
		}

		public override void Setup()
		{
			
		}
	}
}
