using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Common;

namespace CSInteraction.ProgramMessage
{
	[Serializable]
	public class ChoiceWeapon : IMessage
	{
		public TypesWeapon ChooseWeapon { get; private set; }
		public TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.ChoiceWeapon;

		public ChoiceWeapon(TypesWeapon chooseWeapon)
		{
			ChooseWeapon = chooseWeapon;
		}
	}
}
