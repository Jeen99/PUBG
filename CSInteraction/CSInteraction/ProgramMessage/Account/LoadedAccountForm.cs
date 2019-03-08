using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSInteraction.ProgramMessage
{
	[Serializable]
	public class LoadedAccountForm : IMessage
	{
		public TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.LoadedAccountForm;
	}
}
