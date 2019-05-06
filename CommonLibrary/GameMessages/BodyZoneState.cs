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
	public class BodyZoneState : Message
	{
		public BodyZoneState(PointF location, float radius)
		{
			Location = location;
			Radius = radius;
		}

		public override PointF Location { get; }

		public override float Radius { get; }

		public override TypesMessage TypeMessage { get; } = TypesMessage.BodyZoneState;
	}
}
