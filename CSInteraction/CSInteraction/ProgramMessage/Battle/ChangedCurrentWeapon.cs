using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Common;

namespace CSInteraction.ProgramMessage
{
	[Serializable]
	public class ChangedCurrentWeapon : IMessage
	{
		public TypesWeapon NewCurrentWeapon { get; private set; }

		public TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.ChangedCurrentWeapon;

		public ChangedCurrentWeapon(TypesWeapon newCurrentWeapon)
		{
			NewCurrentWeapon = newCurrentWeapon;
		}
	}
}
