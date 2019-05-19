using System.Drawing;

namespace BattleRoayleServer
{
	public interface IBodyZone:IComponent
	{
		PointF Location { get; }
		float Radius { get; }
	}
}