using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.InternalForClient
{
	public class ConnectionBroken:Message
	{
		public override TypesMessage TypeMessage { get; } = TypesMessage.ConnectionBroken;
	}
}
