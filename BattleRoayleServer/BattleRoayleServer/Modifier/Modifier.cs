using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSInteraction.Common;

namespace BattleRoayleServer
{
	public abstract class Modifier : IGameObject
	{
		public Modifier(IGameModel model) : base(model)
		{
		}
		public IGameObject Holder { get; set; }
    }
}