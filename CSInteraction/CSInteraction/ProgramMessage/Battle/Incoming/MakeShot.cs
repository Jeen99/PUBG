using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CSInteraction.ProgramMessage
{
	[Serializable]
	public class MakeShot : IMessage
	{
		public PointF PointOfClick { get; private set; }

		public TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.MakeShot;

		public MakeShot(PointF pointOfClick)
		{
			PointOfClick = pointOfClick;
		}
	}
}
