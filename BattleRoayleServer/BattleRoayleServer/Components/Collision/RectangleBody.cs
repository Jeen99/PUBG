using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using CSInteraction.Common;

namespace BattleRoayleServer
{
	public class RectangleBody : SolidBody
	{
		//диапазоны в которых находятся размеры фигуры 
		private Tuple<float, float> diapasonX;
		public Tuple<float, float> DiapasonX
		{
			get
			{
				if (diapasonX == null)
				{
					float dX = Size.Item1 / 2;
					diapasonX = new Tuple<float, float>(Location.X - dX, Location.X + dX);
				}
				return diapasonX;
			}
			private set { diapasonX = value; }
		}
		private Tuple<float, float> diapasonY;
		public Tuple<float, float> DiapasonY {
			get
			{
				if (diapasonY == null)
				{
					float dY = Size.Item2 / 2;
					diapasonY = new Tuple<float, float>(Location.Y - dY, Location.Y + dY);
				}
				return diapasonY;
			}
			private	set { diapasonY = value; }
		}
		public RectangleBody(GameObject owner, IGameModel gameModel, PointF location, Tuple<float, float> size, TypesSolid typeSolid) 
			: base(owner, gameModel, location, typeSolid)
		{
			Size = size;
			//размещаем объект на игровой карте
			gameModel.Field.Put(this);
		}

		public Tuple<float, float> Size { get; private set; }

		public override TypesSolidBody Type { get; } = TypesSolidBody.Rectangle;

		public override void AppendCoords(float dX, float dY)
		{
			base.AppendCoords(dX, dY);
			DiapasonX = null;
			DiapasonY = null;
		}

		public override IList<Directions> CheckCovered(Tuple<float, float> XDiapason, Tuple<float, float> YDiapason)
		{
			List<Directions> directionsCoveredCells = new List<Directions>();
			//при перемещении дипазоны меняются и поэтому их надо расчитывать заново
			
			if (XDiapason.Item1 > DiapasonX.Item1)
			{
				directionsCoveredCells.Add(Directions.left);

				if (YDiapason.Item1 > DiapasonY.Item1)
					directionsCoveredCells.Add(Directions.left_bottom);
				if(YDiapason.Item2 < DiapasonY.Item2)
					directionsCoveredCells.Add(Directions.left_top);
			}
			if (XDiapason.Item2 < DiapasonX.Item2)
			{
				directionsCoveredCells.Add(Directions.right);

				if (YDiapason.Item1 > DiapasonY.Item1)
					directionsCoveredCells.Add(Directions.right_bottom);
				if (YDiapason.Item2 < DiapasonY.Item2)
					directionsCoveredCells.Add(Directions.right_top);
			}
			if (XDiapason.Item1 < DiapasonX.Item1 && XDiapason.Item2 > DiapasonX.Item2)
			{
				if (YDiapason.Item1 > DiapasonY.Item1)
					directionsCoveredCells.Add(Directions.bottom);
				if (YDiapason.Item2 < DiapasonY.Item2)
					directionsCoveredCells.Add(Directions.top);
			}
			if (directionsCoveredCells.Count > 0)
				return directionsCoveredCells;
			else return null;
		}

		protected override bool CheckCollisionWithCircle(IFieldObject fieldObject)
		{
			RectangleBody rectangleBody = (RectangleBody)fieldObject;
			if (!(DiapasonX.Item1 < rectangleBody.DiapasonX.Item1 && DiapasonX.Item2 > rectangleBody.DiapasonX.Item1) &&
				!(DiapasonX.Item1 < rectangleBody.DiapasonX.Item2 && DiapasonX.Item2 > rectangleBody.DiapasonX.Item2) &&
				!(DiapasonY.Item1 < rectangleBody.DiapasonY.Item1 && DiapasonY.Item2 > rectangleBody.DiapasonY.Item1) &&
				!(DiapasonY.Item1 < rectangleBody.DiapasonY.Item2 && DiapasonY.Item2 > rectangleBody.DiapasonY.Item2))
			{
				return false;
			}
			else return true;
		}

		protected override bool CheckCollisionWithRectangle(IFieldObject fieldObject)
		{
			RectangleBody rectangleBody = (RectangleBody)fieldObject;
			if (!(DiapasonX.Item1 < rectangleBody.DiapasonX.Item1 && DiapasonX.Item2 > rectangleBody.DiapasonX.Item1) &&
				!(DiapasonX.Item1 < rectangleBody.DiapasonX.Item2 && DiapasonX.Item2 > rectangleBody.DiapasonX.Item2) &&
				!(DiapasonY.Item1 < rectangleBody.DiapasonY.Item1 && DiapasonY.Item2 > rectangleBody.DiapasonY.Item1) &&
				!(DiapasonY.Item1 < rectangleBody.DiapasonY.Item2 && DiapasonY.Item2 > rectangleBody.DiapasonY.Item2))
			{
				return false;
			}
			else return true;
		}

		public override void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}