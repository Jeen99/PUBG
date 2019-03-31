using CSInteraction.Common;
using CSInteraction.ProgramMessage;

namespace BattleRoayleServer
{
	public interface IMagazin:IComponent
	{
		TypesWeapon TypeMagazin { get; }
		IBullet GetBullet();
	}
}