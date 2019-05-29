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
	public class MakeShot : Message
	{
		public override ulong ID { get;  set;}

		public override PointF Location { get; }

		public override TypesMessage TypeMessage { get; } = TypesMessage.MakeShot;

		public MakeShot(PointF location)
		{
			Location = location;
		}

		public MakeShot(ulong iD, PointF location)
		{
			ID = iD;
			Location = location;
		}
	}
}
