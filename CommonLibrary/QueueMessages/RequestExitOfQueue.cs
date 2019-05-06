using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.CommonElements;

namespace CommonLibrary.QueueMessages
{	[Serializable]
	public class RequestExitOfQueue : Message
	{ 
		public override TypesMessage TypeMessage { get; } = TypesMessage.RequestExitOfQueue;

	}
}
