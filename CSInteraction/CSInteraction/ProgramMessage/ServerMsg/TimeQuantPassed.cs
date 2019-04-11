using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSInteraction.ProgramMessage
{
	public class TimeQuantPassed : IMessage
	{
		public int QuantTime { get; private set; }

		public TimeQuantPassed(int quantTime = 1)
		{
			QuantTime = quantTime;
		}

		public TypesProgramMessage TypeMessage { get;  } = TypesProgramMessage.TimeQuantPassed;
	}
}
