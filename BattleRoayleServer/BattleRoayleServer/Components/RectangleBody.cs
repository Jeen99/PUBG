using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSInteraction.Common;

namespace BattleRoayleServer
{
	public class RectangleBody:SolidBody
	{
		//диапазоны в которых находятся размеры фигуры 
		private Tuple<double, double> diapasonX;
		public Tuple<double, double> DiapasonX
		{
			get
			{
				if (diapasonX == null)
				{
					double dX = Size.Item1 / 2;
					diapasonX = new Tuple<double, double>(Location.Item1 - dX, Location.Item1 + dX);
				}
				return diapasonX;
			}
			private set { diapasonX = value; }
		}
		private Tuple<double, double> diapasonY;
		public Tuple<double, double> DiapasonY {
			get
			{
				if (diapasonY == null)
				{
					double dY = Size.Item2 / 2;
					diapasonY = new Tuple<double, double>(Location.Item2 - dY, Location.Item2 + dY);
				}
				return diapasonY;
			}
			private	set { diapasonY = value; }
		}
		public RectangleBody(GameObject owner, IGameModel gameModel, Tuple<double, double> location, Tuple<double, double> size, TypesSolid typeSolid) 
			: base(owner, gameModel, location, typeSolid)
		{
			Size = size;
			//размещаем объект на игровой карте
			gameModel.Field.Put(this);
		}

		public Tuple<double, double> Size { get; private set; }

		public override TypesSolidBody Type { get; } = TypesSolidBody.Rectangle;

		public override void AppendCoords(double dX, double dY)
		{
			base.AppendCoords(dX, dY);
			DiapasonX = null;
			DiapasonY = null;
		}

		public override IList<Directions> CheckCovered(Tuple<double, double> XDiapason, Tuple<double, double> YDiapason)
		{
			List<Directions> directionsCoveredCells = new List<Directions>();
			//при перемещении дипазоны меняются и поэтому их надо расчитывать заново
			
			if (XDiapason.Item1 > DiapasonX.Item1)
			{
				directionsCoveredCells.Add(Directions.left);
				if (YDiapason.Item1 > DiapasonY.Item1) directionsCoveredCells.Add(Directions.left_bottom);
				if(YDiapason.Item2 < DiapasonY.Item2) directionsCoveredCells.Add(Directions.left_top);
			}
			if (XDiapason.Item2 < DiapasonX.Item2)
			{
				directionsCoveredCells.Add(Directions.right);
				if (YDiapason.Item1 > DiapasonY.Item1) directionsCoveredCells.Add(Directions.right_bottom);
				if (YDiapason.Item2 < DiapasonY.Item2) directionsCoveredCells.Add(Directions.right_top);
			}
			if (XDiapason.Item1 < DiapasonX.Item1 && XDiapason.Item2 > DiapasonX.Item2)
			{
				if (YDiapason.Item1 > DiapasonY.Item1) directionsCoveredCells.Add(Directions.bottom);
				if (YDiapason.Item2 < DiapasonY.Item2) directionsCoveredCells.Add(Directions.top);
			}
			if (directionsCoveredCells.Count > 0) return directionsCoveredCells;
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