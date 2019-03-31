using CSInteraction.ProgramMessage;

namespace BattleRoayleServer
{
	public interface ICurrentWeapon:IComponent
	{
		IWeapon GetCurrentWeapon { get; }
	}
}