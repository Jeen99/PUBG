using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleRoayleServer
{
	public abstract class Modifier : GameObject
	{
		public Modifier(IModelForComponents model) : base(model)
		{
		}
		public IGameObject Holder { get; set; }
    }
}