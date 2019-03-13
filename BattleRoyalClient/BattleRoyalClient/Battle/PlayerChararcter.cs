using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BattleRoyalClient
{
	class PlayerChararcter
	{

		private Gamer character;
		private PointF LastLocation;
		public ulong ID { get; private set; }
		private BattleModel parent;

		private Size halfVisibleArea = new Size(75,50);

		public PlayerChararcter(ulong iD, BattleModel parent)
		{
			ID = iD;
			this.parent = parent;
			x = new Diapason();
			y = new Diapason();
		}

		/// <summary>
		/// Координаты видимые пользоватем по оси X
		/// </summary>
		private Diapason x;
		public Diapason X
		{
			get { return x; }
		}

		/// <summary>
		/// Координаты видимые пользоватем по оси Y
		/// </summary>
		private Diapason y;
		public Diapason Y
		{
			get { return y; }
		}

		public void CharacterChange()
		{
			if (character == null)
			{
				character = (Gamer)parent.GameObjects[ID];
			}

			if (character.Location != LastLocation)
			{
				LastLocation = character.Location;
				x.Left = LastLocation.X - halfVisibleArea.Width;
				x.Right = LastLocation.X + halfVisibleArea.Width;
				y.Left = LastLocation.Y - halfVisibleArea.Height;
				y.Right = LastLocation.Y + halfVisibleArea.Height;
			}
		}

		public List<IGameObject> VisibleObjects
		{
			get
			{
				List<IGameObject> gameObjects = new List<IGameObject>();
				foreach (var item in parent.GameObjects)
				{
					IGameObject gameObject = item.Value;
					if(x.Left <= gameObject.Location.X && gameObject.Location.X <= x.Right 
						&& y.Left <= gameObject.Location.Y && gameObject.Location.Y <= y.Right)
					{
						gameObjects.Add(gameObject);
					}
				}
				return gameObjects;
			}
		}

		//возрващает расстояние на вышла камера за границы карты
		public OverflowMap GetOverflow
		{
			get
			{
				float left;
				if (x.Left < 0) left = -x.Left;
				else left = 0;

				float right;
				if (x.Right > parent.SizeMap.Width) right = x.Right - parent.SizeMap.Width;
				else right = 0;

				float bottom;
				if (y.Left < 0) bottom = -y.Left;
				else bottom = 0;

				float top;
				if (y.Right > parent.SizeMap.Height) top = y.Right - parent.SizeMap.Height;
				else top = 0;

				return new OverflowMap(top, bottom, left, right);
			}
		}
	}

	struct OverflowMap
	{
		public float Top { get; }
		public float Bottom { get; }
		public float Left { get; }
		public float Right { get; }

		public OverflowMap(float top, float bottom, float left, float right) : this()
		{
			Top = top;
			Bottom = bottom;
			Left = left;
			Right = right;
		}
	}
}
