﻿using System;
using System.Collections.Generic;
using System.Drawing;
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

		public override TypesGameObject Type => throw new NotImplementedException();

        public override TypesBehaveObjects TypesBehave => throw new NotImplementedException();

		public PointF GetLocation()
		{
			throw new NotImplementedException();
		}

		public void PerformAction(IMessage action)
        {
            throw new NotImplementedException();
        }

		
	}
}