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
		public int Kills { get; private set; } = 0;
		public TimeSpan TimeInBattle { get; private set; } = new TimeSpan();

		public Statistics(IGameObject parent) : base(parent)
		{

		}

		public override void UpdateComponent(IMessage msg)
		{
			if (msg == null)
			{
				Log.AddNewRecord("Получено null сообщение в компоненте Shot");
				return;
			}

			switch (msg.TypeMessage)
			{
				case TypesProgramMessage.TimeQuantPassed:
					Handler_TimeQuantPassed(msg as TimeQuantPassed);
					break;
				case TypesProgramMessage.MakedKill:
					Handler_MakedKill();
					break;
			}
		}

		private void Handler_MakedKill()
		{
			Kills++;
		}

		private void Handler_TimeQuantPassed(TimeQuantPassed msg)
		{
			TimeInBattle = TimeInBattle.Add(new TimeSpan(0, 0, 0, 0, msg.QuantTime));
		}

		public override void Setup()
		{
			
		}
	}
}
