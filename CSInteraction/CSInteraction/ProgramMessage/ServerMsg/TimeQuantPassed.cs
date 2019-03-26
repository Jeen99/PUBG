using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSInteraction.ProgramMessage
{
	public class TimeQuantPassed : IMessage
	{
		public double QuantTime { get; private set; }

		public TimeQuantPassed(double quantTime = 1.0)
		{
			QuantTime = quantTime;
		}

		public TypesProgramMessage TypeMessage { get;  } = TypesProgramMessage.TimeQuantPassed;
	}
}
