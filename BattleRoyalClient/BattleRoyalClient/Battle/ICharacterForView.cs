using System.Drawing;
using CommonLibrary.CommonElements;

namespace BattleRoyalClient
{
	interface ICharacterForView
	{
		IGamerForView Character { get; }
		float HP { get; }
		ulong ID { get; }
		PointF Location { get; }
		TypesWeapon[] Weapons { get; }

		event CharacterChange Event_CharacterChange;
	}
}