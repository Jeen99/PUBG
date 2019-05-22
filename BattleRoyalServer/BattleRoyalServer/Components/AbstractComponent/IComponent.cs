using CommonLibrary;

namespace BattleRoyalServer
{
	public interface IComponent
	{
		IGameObject Parent { get; }
		IMessage State { get; }

		void Dispose();
		void Setup();
	}
}