using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using CommonLibrary.CommonElements;

namespace CommonLibrary.GameMessages
{
	[Serializable]
	public class FieldState : Message
	{
		public override SizeF Size { get; }

		public override TypesMessage TypeMessage { get; } = TypesMessage.FieldState;

		public FieldState(SizeF size)
		{
			Size = size;
		}
	}
}
