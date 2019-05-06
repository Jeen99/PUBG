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
	public class BodyState : Message
	{
		public override RectangleF Shape { get; }

		public override TypesMessage TypeMessage { get; } = TypesMessage.BodyState;

		public BodyState(RectangleF shape)
		{
			Shape = shape;
		}
	}
}
