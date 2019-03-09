using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BattleRoyalClient
{
	static class Painter
	{
		public static void DrawBox(Graphics gr, Tuple<double, double> Location)
		{
			using (Brush NewBrush = new  SolidBrush(Color.Brown))
			{
				gr.FillRectangle(NewBrush, (float)Location.Item1, (float)Location.Item2, 3, 3);
			}
				
		}

		public static void DrawStone(Graphics gr, Tuple<double, double> Location,  double Radius)
		{
			using (Brush NewBrush = new SolidBrush(Color.Gray))
			{
				gr.FillEllipse(NewBrush, (float)(Location.Item1 - Radius/2), (float)(Location.Item2 - Radius/2), 
					(float)Radius, (float)Radius);
			}
		}

	}
}
