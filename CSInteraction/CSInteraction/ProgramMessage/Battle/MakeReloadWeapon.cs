﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSInteraction.ProgramMessage
{
	[Serializable]
	public class MakeReloadWeapon : IMessage
	{
		public TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.MakeReloadWeapon;
	}
}
