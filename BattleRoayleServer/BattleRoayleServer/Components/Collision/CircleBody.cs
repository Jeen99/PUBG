using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace BattleRoayleServer
{
	public class CircleBody : SolidBody
	{
		private Tuple<float, float> diapasonX;
		public Tuple<float, float> DiapasonX
		{
			get
			{
				if (diapasonX == null)
					diapasonX = new Tuple<float, float>(Location.X - Radius, Location.Y + Radius);
				return diapasonX;
			}
			private set
			{
				diapasonX = value;
			}
		}
		private Tuple<float, float> diapasonY;
		public Tuple<float, float> DiapasonY
		{
			get
			{
				if (diapasonY == null)
					diapasonY = new Tuple<float, float>(Location.X - Radius, Location.Y + Radius);
				return diapasonY;
			}
			private set
			{
				diapasonY = value;
			}
		}

		public CircleBody(GameObject owner, IGameModel gameModel, PointF location, float radius, TypesSolid typeSolid)
			: base(owner, gameModel, location, typeSolid)
		{
			Radius = radius;
		}

		public float Radius { get; private set; }

		public override TypesSolidBody Type { get; } = TypesSolidBody.Circle;

		public override void AppendCoords(float dX, float dY)
		{
			base.AppendCoords(dX, dY);
			DiapasonX = null;
			DiapasonY = null;
		}

		public override IList<Directions> CheckCovered(Tuple<float, float> XDiapason, Tuple<float, float> YDiapason)
		{
			List<Directions> directionsCoveredCells = new List<Directions>();

			if (XDiapason.Item1 > DiapasonX.Item1)
			{
				directionsCoveredCells.Add(Directions.left);

				float RasnX = (float)Math.Pow(XDiapason.Item1 - Location.X, 2);
				float KvRadius = (float)Math.Pow(Radius, 2);

				if (RasnX + Math.Pow(YDiapason.Item1 - Location.Y, 2) < KvRadius)
				{
					directionsCoveredCells.Add(Directions.left_bottom);
				}
				if (RasnX + Math.Pow(YDiapason.Item2 - Location.Y, 2) < KvRadius)
				{
					directionsCoveredCells.Add(Directions.left_top);
				}
			}
			if (XDiapason.Item2 < DiapasonX.Item2)
			{
				directionsCoveredCells.Add(Directions.right);

				float RasnX = (float)Math.Pow(XDiapason.Item2 - Location.X, 2);
				float KvRadius = (float)Math.Pow(Radius, 2);

				if (RasnX + Math.Pow(YDiapason.Item1 - Location.Y, 2) < KvRadius)
				{
					directionsCoveredCells.Add(Directions.right_bottom);
				}
				if (RasnX + Math.Pow(YDiapason.Item2 - Location.Y, 2) < KvRadius)
				{
					directionsCoveredCells.Add(Directions.right_top);
				}
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
			else
				return null;
		}

		protected override bool CheckCollisionWithCircle(IFieldObject fieldObject)
		{
			CircleBody circleBody = (CircleBody)fieldObject;
			//определяем расстоняие между 2 центрами окружностей
			float distance = (float)Math.Sqrt(Math.Pow(this.Location.X - circleBody.Location.X, 2)
				+ Math.Pow(this.Location.Y - circleBody.Location.Y, 2));

			if (distance < circleBody.Radius + this.Radius)
				return true;
			else
				return false;
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