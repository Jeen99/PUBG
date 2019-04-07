using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CSInteraction.ProgramMessage
{
	[Serializable]
	public class BodyZoneState : IMessage
	{
		public BodyZoneState(PointF location, float radius)
		{
			Location = location;
			Radius = radius;
		}

		public TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.BodyZoneState;

		public PointF Location { get; private set; }

		public float Radius { get; private set; }

	}
}
