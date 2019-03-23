using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Common;

namespace CSInteraction.ProgramMessage
{
	[Serializable]
	public class AddWeapon : IMessage
	{
		public TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.AddWeapon;
		public TypesGameObject TypeWeapon { get; private set; }

		public AddWeapon(TypesGameObject typeWeapon)
		{
			TypeWeapon = typeWeapon;
		}
	}
}
