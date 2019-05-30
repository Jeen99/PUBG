using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

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

		public MakeShot(ulong iD, float angle)
		{
			ID = iD;

			double radian = angle * (System.Math.PI / 180);

			Location = new PointF()
			{
				X = (float)(System.Math.Cos(radian)),
				Y = -(float)(System.Math.Sin(radian))
			};
		}
	}
}
