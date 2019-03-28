using CSInteraction.Common;

namespace BattleRoayleServer
{
	public interface ICollector:IComponent
	{
		IWeapon GetWeapon(TypesWeapon typeWeapon);
		void SetNewParent(LootBox lootBox);
	}
}