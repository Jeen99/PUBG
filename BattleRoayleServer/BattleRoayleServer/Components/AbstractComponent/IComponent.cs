using CSInteraction.ProgramMessage;

namespace BattleRoayleServer
{
	public interface IComponent
	{
		GameObject Parent { get; }
		IMessage State { get; }

		void Dispose();
		void UpdateComponent(IMessage msg);
	}
}