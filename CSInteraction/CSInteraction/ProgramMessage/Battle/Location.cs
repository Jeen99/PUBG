using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSInteraction.ProgramMessage
{
	[Serializable]
	public class Location : IMessage
	{
		public Tuple<double, double> LocationBody { get; private set; }
		public TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.Location;

		public Location(Tuple<double, double> location)
		{
			LocationBody = location;
		}
	}
}
