using System.Drawing;
using System.Windows.Media.Media3D;
using CSInteraction.Common;

namespace BattleRoyalClient
{
	interface IModelObject
	{
		double Angle { get; }
		PointF Location { get; }
		Point3D Location3D { get; }
		RectangleF Shape { get; set; }
		SizeF Size { get; }
		TypesGameObject Type { get; }
	}
}