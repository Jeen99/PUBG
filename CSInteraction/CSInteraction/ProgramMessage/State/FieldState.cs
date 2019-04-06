using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CSInteraction.ProgramMessage
{
	[Serializable]
	public class FieldState : IMessage
	{
		public TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.FieldState;

		public SizeF Size { get; private set; }

		public FieldState(SizeF size)
		{
			Size = size;
		}
	}
}
