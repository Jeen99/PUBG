using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleRoayleServer
{
	public class CircleBody : SolidBody
	{
		private Tuple<double, double> diapasonX;
		public Tuple<double, double> DiapasonX
		{
			get
			{
				if (diapasonX == null)
					diapasonX = new Tuple<double, double>(Location.Item1 - Radius, Location.Item1 + Radius);
				return diapasonX;
			}
			private set
			{
				diapasonX = value;
			}
		}
		private Tuple<double, double> diapasonY;
		public Tuple<double, double> DiapasonY
		{
			get
			{
				if (diapasonY == null)
					diapasonY = new Tuple<double, double>(Location.Item2 - Radius, Location.Item2 + Radius);
				return diapasonY;
			}
			private set
			{
				diapasonY = value;
			}
		}

		public CircleBody(GameObject owner, IGameModel gameModel, Tuple<double, double> location, double radius, TypesSolid typeSolid)
			: base(gameModel, owner, location, typeSolid)
		{
			Radius = radius;
		}

		public double Radius { get; private set; }

		//необходимо реализовать 
		public override ComponentState State { get; }

		public override TypesSolidBody Type { get; } = TypesSolidBody.Circle;

		public override void AppendCoords(double dX, double dY)
		{
			base.AppendCoords(dX, dY);
			DiapasonX = null;
			DiapasonY = null;
		}

		public override IList<Directions> CheckCovered(Tuple<double, double> XDiapason, Tuple<double, double> YDiapason)
		{
			List<Directions> directionsCoveredCells = new List<Directions>();

			if (XDiapason.Item1 > DiapasonX.Item1)
			{
				directionsCoveredCells.Add(Directions.left);
				double RasnX = Math.Pow(XDiapason.Item1 - Location.Item1, 2);
				double KvRadius = Math.Pow(Radius, 2);
				if (RasnX + Math.Pow(YDiapason.Item1 - Location.Item2, 2) < KvRadius)
				{
					directionsCoveredCells.Add(Directions.left_bottom);
				}
				if (RasnX + Math.Pow(YDiapason.Item2 - Location.Item2, 2) < KvRadius)
				{
					directionsCoveredCells.Add(Directions.left_top);
				}
			}
			if (XDiapason.Item2 < DiapasonX.Item2)
			{
				directionsCoveredCells.Add(Directions.right);
				double RasnX = Math.Pow(XDiapason.Item2 - Location.Item1, 2);
				double KvRadius = Math.Pow(Radius, 2);
				if (RasnX + Math.Pow(YDiapason.Item1 - Location.Item2, 2) < KvRadius)
				{
					directionsCoveredCells.Add(Directions.right_bottom);
				}
				if (RasnX + Math.Pow(YDiapason.Item2 - Location.Item2, 2) < KvRadius)
				{
					directionsCoveredCells.Add(Directions.right_top);
				}
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
			CircleBody circleBody = (CircleBody)fieldObject;
			//определяем расстоняие между 2 центрами окружностей
			double distance = Math.Sqrt(Math.Pow(this.Location.Item1 - circleBody.Location.Item1, 2)
				+ Math.Pow(this.Location.Item2 - circleBody.Location.Item2, 2));
			if (distance < circleBody.Radius + this.Radius) return true;
			else return false;

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
	}
}