using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Common;
using CSInteraction.ProgramMessage;

namespace BattleRoayleServer
{
	 class Box:GameObject
	{
		public Box(IGameModel context, PointF location, Size size): base()
		{
			this.Components = new List<Component>(1);
			Components.Add(new SolidBody(this, context, new RectangleF(location, size), TypesSolid.Solid));
		}

		public override TypesBehaveObjects TypesBehave { get; } = TypesBehaveObjects.Passive;

		public override TypesGameObject Type { get;  } = TypesGameObject.Box;
	}


}
