using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSInteraction.ProgramMessage;

namespace BattleRoayleServer
{
    public class Gamer : GameObject, IPlayer
    {
		public Gamer():base()
		{

		}

		public Tuple<int, int> GetLocation()
        {
            throw new NotImplementedException();
        }

        public void PerformAction(IMessage action)
        {
            throw new NotImplementedException();
        }

		
	}
}