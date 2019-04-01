using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Common;

namespace CSInteraction.ProgramMessage
{
	[Serializable]
	public class ChangedCurrentWeapon : IMessage, IOutgoing
	{
		public ChangedCurrentWeapon(ulong iD, TypesWeapon newCurrentWeapon)
		{
			ID = iD;
			NewCurrentWeapon = newCurrentWeapon;
		}

		public ulong ID { get; private set; }

		public TypesWeapon NewCurrentWeapon { get; private set; }

		public TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.ChangedCurrentWeapon;

		
	}
}
