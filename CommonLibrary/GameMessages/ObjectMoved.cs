using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.CommonElements;

namespace CommonLibrary.GameMessages
{
	[Serializable]
	public class ObjectMoved : Message
	{
		public override ulong ID { get; }

		public override PointF Location { get; }

		public override TypesMessage TypeMessage { get; } = TypesMessage.ObjectMoved;

		public ObjectMoved(ulong iD, PointF newLocation)
		{
			ID = iD;
			Location = newLocation;
		}
	}
}
