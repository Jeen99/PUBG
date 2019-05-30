using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyalServer
{
	/// <summary>
	/// Видимая зона игрока. Используется для фильтрации сообщений.
	/// </summary>
	public class VisibleArea
	{
		private RectangleF _visibleArea = new RectangleF(0, 0, WidthVisibleArea, HeightVisibleArea);
		private readonly IPlayer _player;

		private static readonly int WidthVisibleArea = 160;
		private static readonly int HeightVisibleArea = 160;

		public VisibleArea(IPlayer player)
		{
			this._player = player;
		}

		public RectangleF Area
		{
			get
			{
				//определяем положение, чтобы игрок был примерно в центре видимой области
				var location = new PointF((_player.Location.X - WidthVisibleArea / 2.0F),
					(_player.Location.Y - HeightVisibleArea / 2.0F));
				_visibleArea.Location = location;

				return _visibleArea;
			}
		}
	}
}
