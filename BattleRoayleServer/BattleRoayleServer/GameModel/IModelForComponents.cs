using System.Collections.Generic;
using System.Drawing;
using Box2DX.Dynamics;
using CommonLibrary;

namespace BattleRoayleServer
{
	public interface IModelForComponents
	{
		World Field { get; }
		IList<IPlayer> Players { get; }
		void AddOrUpdateGameObject(IGameObject gameObject);
		void RemoveGameObject(IGameObject gameObject);
		void AddEvent(IMessage message);
	}
}