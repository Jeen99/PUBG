using CommonLibrary.CommonElements;
using System.Drawing;

namespace BattleRoyalClient
{
	interface ICharacterForControler
	{
		ulong ID { get; }
		void ChangeCurrentWeapon(TypesWeapon weapon);
		void AddWeapon(TypesWeapon weapon);
		void ChangeHP(float newHP);
		void OnChangeCharacter(TypesChangeCharacter typeChange);
		void ChangeLocation(PointF location);
		void Create(Gamer gamer);
	}
}