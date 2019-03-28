using System.Collections.Generic;
using System.Drawing;
using Box2DX.Dynamics;
using CSInteraction.ProgramMessage;

namespace BattleRoayleServer
{
	public interface ISolidBody:IComponent
	{
		Body Body { get; }
		List<ISolidBody> CoveredObjects { get; }
		RectangleF Shape { get; }
		TypesSolid TypeSolid { get; }
		void BodyDelete();
		List<ISolidBody> GetPickUpObjects();
	}
}