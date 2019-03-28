using CSInteraction.ProgramMessage;

namespace BattleRoayleServer
{
	public interface ICurrentWeapon:IComponent
	{
		Weapon GetCurrentWeapon { get; }
	}
}