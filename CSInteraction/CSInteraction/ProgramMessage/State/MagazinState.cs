using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSInteraction.ProgramMessage
{
	[Serializable]
	public class MagazinState : IMessage
	{
		public int BulletInMagazin {get; private set;}
		public TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.MagazinState;

		public MagazinState(int bulletInMagazin)
		{
			BulletInMagazin = bulletInMagazin;
		}
	}
}
