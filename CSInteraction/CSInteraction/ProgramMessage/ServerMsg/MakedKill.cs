﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSInteraction.ProgramMessage
{
	public class MakedKill : IMessage
	{
		public TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.MakedKill;
	}
}
