using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.CommonElements;

namespace CommonLibrary.GameMessages
{
	[Serializable]
	public class TimeQuantPassed : Message
	{
		public TimeQuantPassed(int timePassed)
		{
			TimePassed = timePassed;
		}

		public override int TimePassed { get; }

		public override TypesMessage TypeMessage { get; } = TypesMessage.TimeQuantPassed;
	}
}
