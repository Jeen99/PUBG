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
		IWeaponCharacterForView GetWeapon(uint index);

		int CountWeapons();
		event CharacterChange Event_CharacterChange;
	}
}