using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSInteraction.ProgramMessage
{
	[Serializable]
	public class MakeShot : IMessage
	{
		public float Angle { get; private set; }

		public TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.MakeShot;

		public MakeShot(float angle)
		{
			Angle = angle;
		}
	}
}
