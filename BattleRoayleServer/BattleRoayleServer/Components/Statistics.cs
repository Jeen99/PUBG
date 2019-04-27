using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.CommonElements;
using CommonLibrary.GameMessages;
using CommonLibrary;

namespace BattleRoayleServer
{
	public class Statistics : Component
	{
		public int Kills { get; private set; } = 0;
		public TimeSpan TimeInBattle { get; private set; } = new TimeSpan();
		public bool GamerDied { get; private set; } = false;

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
				case TypesMessage.TimeQuantPassed:
					Handler_TimeQuantPassed(msg);
					break;
				case TypesMessage.MakedKill:
					Handler_MakedKill();
					break;
				case TypesMessage.GamerDied:
					Handler_GamerDied();
					break;
			}
		}

		private void Handler_GamerDied()
		{
			GamerDied = true;
		}

		private void Handler_MakedKill()
		{
			Kills++;
		}

		private void Handler_TimeQuantPassed(IMessage msg)
		{
			TimeInBattle = TimeInBattle.Add(new TimeSpan(0, 0, 0, 0, msg.TimePassed));
		}

		public override void Setup()
		{
			
		}

		public override void Dispose()
		{
			Parent.Model.AddOutgoingMessage(new EndGame(Parent.ID, GamerDied, Kills, TimeInBattle));
		}
	}
}
