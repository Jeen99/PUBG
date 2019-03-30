using System.Collections.Generic;
using System.Drawing;
using Box2DX.Dynamics;
using CSInteraction.ProgramMessage;

namespace BattleRoayleServer
{
	public interface IModelForComponents
	{
		World Field { get; }
		void AddGameObject(IGameObject gameObject);
		List<IGameObject> GetPickUpObjects(RectangleF shapePlayer);
		void RemoveGameObject(IGameObject gameObject);
		void AddLoot(IGameObject gameObject);
		void AddEvent(IMessage message);
		void RemoveLoot(IGameObject gameObject);
	}
}