using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSInteraction.ProgramMessage
{
	[Serializable]
	public class Location : IMessage
	{
		public PointF LocationBody { get; private set; }
		public TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.Location;

		public Location(PointF location)
		{
			LocationBody = location;
		}
	}
}
