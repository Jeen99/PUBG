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

		public static void Draw(IGameObject gameObject, Graphics gr)
		{
			switch (gameObject.Type)
			{
				case TypesGameObject.Player:
					DrawGamer(gameObject as Gamer, gr);
					break;
			}
		}

		private static void DrawGamer(Gamer gamer, Graphics gr)
		{	
			gr.DrawImage(Properties.Recources.Gamer, new RectangleF(
				ConvertPosition.ConvertToViewLocation(gamer.Location), 
				new Size(30, 30)));
			
		}

	}
}
