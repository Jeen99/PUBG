using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CSInteraction.ProgramMessage
{
	[Serializable]
	public class ThrowGrenade : IMessage
	{
		public ThrowGrenade(PointF pointOfThrow)
		{
			PointOfThrow = pointOfThrow;
		}

		public PointF PointOfThrow { get; private set; }

		public TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.ThrowGrenade;
	}
}
