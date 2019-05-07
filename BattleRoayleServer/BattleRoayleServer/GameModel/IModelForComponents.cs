using System.Collections.Generic;
using System.Drawing;
using Box2DX.Dynamics;
using CommonLibrary;
using Box2DX.Collision;

namespace BattleRoayleServer
{
	public interface IModelForComponents
	{
		World Field { get; }
		IList<IPlayer> Players { get; }
		List<SolidBody> GetMetedObjects(Segment ray);
		void AddOrUpdateGameObject(IGameObject gameObject);
		void RemoveGameObject(IGameObject gameObject);
		void AddOutgoingMessage(IMessage message);
	}
}