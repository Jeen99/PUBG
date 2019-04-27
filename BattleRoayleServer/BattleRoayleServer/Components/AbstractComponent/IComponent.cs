using CommonLibrary;

namespace BattleRoayleServer
{
	public interface IComponent
	{
		IGameObject Parent { get; }
		IMessage State { get; }

		void Dispose();
		void Setup();
	}
}