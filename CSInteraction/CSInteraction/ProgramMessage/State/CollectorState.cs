using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSInteraction.ProgramMessage
{
	[Serializable]
	public class CollectorState : IMessage
	{
		public List<IMessage> Modifiers { get; private set; }
		public List<IMessage> Weapons { get; private set; }
		public TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.CollectorState;

		public CollectorState(List<IMessage> modifiers, List<IMessage> weapons)
		{
			Modifiers = modifiers;
			Weapons = weapons;
		}
	}
}
