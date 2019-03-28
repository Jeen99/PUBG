using CSInteraction.Common;
using CSInteraction.ProgramMessage;

namespace BattleRoayleServer
{
	public interface IGameObject
	{
		DictionaryComponent Components { get; }
		bool Destroyed { get; }
		ulong ID { get; }
		IGameModel Model { get; }
		IMessage State { get; }
		TypesGameObject Type { get; }
		TypesBehaveObjects TypesBehave { get; }

		event GameObjectDeleted EventGameObjectDeleted;

		void Dispose();
		void SendMessage(IMessage msg);
		void Update(TimeQuantPassed quantPassed = null);
	}
}