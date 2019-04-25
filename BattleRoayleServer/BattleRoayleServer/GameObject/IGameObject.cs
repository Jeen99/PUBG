using CommonLibrary;
using CommonLibrary.CommonElements;

namespace BattleRoayleServer
{
	public interface IGameObject
	{
		DictionaryComponent Components { get; }
		bool Destroyed { get; }
		ulong ID { get; }
		IModelForComponents Model { get; }
		IMessage State { get; }
		TypesGameObject Type { get; }
		TypesBehaveObjects TypesBehave { get; }

		//event GameObjectDeleted EventGameObjectDeleted;

		void Setup();
		void Dispose();
		void SetDestroyed();
		void Update(IMessage msg);
	}
}