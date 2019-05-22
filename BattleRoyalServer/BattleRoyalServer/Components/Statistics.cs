using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.CommonElements;
using CommonLibrary.GameMessages;
using CommonLibrary;

namespace BattleRoyalServer
{
	public class Statistics : Component
	{
		public int Kills { get; private set; } = 0;
		public TimeSpan TimeInBattle { get; private set; } = new TimeSpan();
		public bool GamerDied { get; private set; } = false;

		public Statistics(IGameObject parent) : base(parent)
		{

		}

		private void Handler_GamerDied(IMessage msg)
		{
			GamerDied = true;
		}

		private void Handler_MakedKill(IMessage msg)
		{
			Kills++;
		}

		private void Handler_TimeQuantPassed(IMessage msg)
		{
			TimeInBattle = TimeInBattle.Add(new TimeSpan(0, 0, 0, 0, msg.TimePassed));
		}

		public override void Setup()
		{
			Parent.Received_TimeQuantPassed += Handler_TimeQuantPassed;
			Parent.Received_GamerDied += Handler_GamerDied;
			Parent.Received_MakedKill += Handler_MakedKill;
		}

		public override void Dispose()
		{
			Parent.Model.AddOutgoingMessage(new EndGame(Parent.ID, GamerDied, Kills, TimeInBattle));
			
			Parent.Received_TimeQuantPassed -= Handler_TimeQuantPassed;
			Parent.Received_GamerDied -= Handler_GamerDied;
			Parent.Received_MakedKill -= Handler_MakedKill;
		}
	}
}
