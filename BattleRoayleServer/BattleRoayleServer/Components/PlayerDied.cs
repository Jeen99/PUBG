using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.ProgramMessage;

namespace BattleRoayleServer
{
	public class PlayerDied : Component
	{
		public PlayerDied(IGameObject parent) : base(parent)
		{
		}

		public override void UpdateComponent(IMessage msg)
		{

		}

		//выполняет некоторые действия при смерти игрока
		public override void Dispose()
		{
			//сохраняем статистику игрока
			BDAccounts.AddToStatistic(new AchievementsOfBattle());
		}

		public override void Setup()
		{
			
		}
	}
}
