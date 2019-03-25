using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using CSInteraction.Common;

namespace BattleRoyalClient
{
	static class Painter
	{
		private static PointF CenterScreen = new  PointF(750, 500);
		public static void Draw(GameObject gameObject, Graphics gr, PointF startAxises )
		{
			switch (gameObject.Type)
			{
				case TypesGameObject.Player:
					DrawGamer(gameObject as Gamer, gr);
					break;
				case TypesGameObject.Stone:
					DrawStone(gameObject as Stone, gr, startAxises);
					break;
				case TypesGameObject.Box:
					DrawBox(gameObject as Box, gr, startAxises);
					break;
			}
		}
		private static void DrawBox(Box box, Graphics gr, PointF startAxises)
		{
			
			gr.DrawImage(Properties.Recources.Box, new RectangleF(
				ConvertPosition.ConvertToViewLocation(box.Shape.Location, startAxises), 
				ConvertPosition.ConvertToViewSize(box.Shape.Size)));

		}

		private static void DrawStone(Stone stone, Graphics gr, PointF startAxises)
		{
			gr.DrawImage(Properties.Recources.Stone, new RectangleF(
				ConvertPosition.ConvertToViewLocation(stone.Shape.Location, startAxises),
				ConvertPosition.ConvertToViewSize(stone.Shape.Size)));
		}

		private static void DrawGamer(Gamer gamer, Graphics gr)
		{
			if (CenterScreen == null)
			{
				CenterScreen = new PointF(750 - (gamer.Shape.Width/2), 500 - (gamer.Shape.Height / 2));
			}
			gr.DrawImage(Properties.Recources.Gamer, new RectangleF(CenterScreen,
				ConvertPosition.ConvertToViewSize(gamer.Shape.Size)));			
		}

	}
}
