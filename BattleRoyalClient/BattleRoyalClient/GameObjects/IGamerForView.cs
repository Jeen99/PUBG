using CommonLibrary.CommonElements;

namespace BattleRoyalClient
{
	interface IGamerForView:IModelObject
	{
		TypesWeapon CurrentWeapon { get; set; }
	}
}