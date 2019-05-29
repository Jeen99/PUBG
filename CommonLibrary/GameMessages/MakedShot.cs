﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.CommonElements;

namespace CommonLibrary.GameMessages
{
	[Serializable]
	public class MakedShot : Message
	{
		public override ulong ID { get;  set;}

		public override float Angle { get;}

		public override float Distance { get; }

		public override TypesMessage TypeMessage { get; } = TypesMessage.MakedShot;

		public MakedShot(ulong iD, float angle, float distance)
		{
			ID = iD;
			Angle = angle;
			Distance = distance;
		}
	}
}
