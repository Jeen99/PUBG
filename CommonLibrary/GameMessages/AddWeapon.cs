using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.CommonElements;


namespace CommonLibrary.GameMessages
{
	[Serializable]
	public class AddWeapon : Message
	{
		public override ulong ID { get; set; }

		public override TypesWeapon TypeWeapon { get; }

		public override TypesMessage TypeMessage { get; } = TypesMessage.AddWeapon;

		public override List<IMessage> InsertCollections { get; }

		public AddWeapon(ulong iD, TypesWeapon typeWeapon, List<IMessage> insertCollections)
		{
			ID = iD;
			TypeWeapon = typeWeapon;
			InsertCollections = insertCollections;
		}
	}
}
