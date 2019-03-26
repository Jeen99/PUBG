using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSInteraction.ProgramMessage
{
	[Serializable]
	public class HealthyState : IMessage
	{
		public float HP { get; private set; }

		public TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.HealthyState;

		public HealthyState(float hP)
		{
			HP = hP;
		}
	}
}
