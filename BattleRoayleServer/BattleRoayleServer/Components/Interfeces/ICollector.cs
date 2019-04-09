using CSInteraction.Common;

namespace BattleRoayleServer
{
	public interface ICollector:IComponent
	{
		Weapon GetWeapon(TypesWeapon typeWeapon);
		void SetNewParent(LootBox lootBox);
	}
}