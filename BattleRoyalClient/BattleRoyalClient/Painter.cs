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
		private static Bitmap BoxImage;
		private static Bitmap StoneImage;
		public static void DrawBox(Graphics gr, PointF Location)
		{
				if (BoxImage == null)
				{
					BoxImage = new Bitmap(Properties.Recources.Box);
				}
				gr.DrawImage(BoxImage, new RectangleF(Location,  new Size(30,30)));
				
		}

		public static void DrawStone(Graphics gr, PointF Location, Size size)
		{	
				if (StoneImage == null)
				{
					StoneImage = new Bitmap(Properties.Recources.Stone); 
				}
				gr.DrawImage(StoneImage, new RectangleF(Location,  size));
		}

	}
}
