using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.CommonElements;

namespace CommonLibrary
{
	[Serializable]
	public class ChangeBulletInWeapon:Message
	{
		public override TypesMessage TypeMessage { get; } = TypesMessage.ChangeBulletInWeapon;
		public override TypesWeapon TypeWeapon { get; }
		public override int Count { get; }
		public override ulong ID { get;  set;}

		public ChangeBulletInWeapon(TypesWeapon typeWeapon, int count)
		{
			TypeWeapon = typeWeapon;
			Count = count;
		}

		public ChangeBulletInWeapon(ulong iD, TypesWeapon typeWeapon, int count)
		{
			ID = iD;
			TypeWeapon = typeWeapon;
			Count = count;
		}
	}
}
