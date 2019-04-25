﻿using System.Drawing;
using System.Windows.Media.Media3D;
using CommonLibrary.CommonElements;

namespace BattleRoyalClient
{
	public interface IModelObject
	{
		double Angle { get; }
		PointF Location { get; }
		Point3D Location3D { get; }
		RectangleF Shape { get; set; }
		SizeF Size { get; }
		string TextureName { get; }
		ulong ID { get; }
		TypesGameObject Type { get; }
		void Update(RectangleF rectangle, double Angle = 0);
		void Update(PointF location, double Angle = 0);
		void Update(double angle);
		
	}
}