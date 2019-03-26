using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSInteraction.ProgramMessage
{
	[Serializable]
	public class BodyState : IMessage
	{
		public RectangleF Shape { get; }
		public TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.BodyState;

		public BodyState(RectangleF shape)
		{
			Shape = shape;
		}
	}
}
