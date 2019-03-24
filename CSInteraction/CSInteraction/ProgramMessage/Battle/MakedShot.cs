using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSInteraction.ProgramMessage
{
	[Serializable]
	public class MakedShot : IMessage
	{
		public ulong ID { get; set; }

		public TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.MakedShot;

		public MakedShot(ulong iD)
		{
			ID = iD;
		}
	}
}
