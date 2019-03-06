using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleRoayleServer
{
    public class RoyalField:IField
    {
        /// <summary>
        /// Задано статически
        /// </summary>
        private CellField[,] content;
        /// <summary>
        /// Размер стороны карты(карта квадратная)
        /// </summary>
        private const int lengthOfSide = 100;
		/// <summary>
		/// Длина стороны клетки
		/// </summary>
		private const int lengthOfSideCell = 10;

		public RoyalField()
		{
			content = new CellField[lengthOfSide, lengthOfSide];
			for (int i = 0; i < lengthOfSide; i++)
			{
				for (int j = 0; j < lengthOfSide; j++)
				{
					content[i, j] = new CellField(new Tuple<double, double>(i * lengthOfSideCell, j * lengthOfSideCell));
				}
			}
		}

		/// <summary>
		/// Определяет клетки, которые покрывает данный объект
		/// </summary>
		private List<CellField> GetCovered(IFieldObject fieldObject)
		{
			//определяем центральную клетку, на которых находиться данный объект
			int X = (int)Math.Truncate(fieldObject.Location.Item1 / lengthOfSideCell);
			int Y = (int)Math.Truncate(fieldObject.Location.Item2 / lengthOfSideCell);

			IList<Directions> directionsCoveredCells = fieldObject.CheckCovered(new Tuple<double, double>(X * lengthOfSideCell, (X + 1) * lengthOfSideCell),
				new Tuple<double, double>(Y * lengthOfSideCell, (Y + 1) * lengthOfSideCell));

			List<CellField> coveredCells;
			if (directionsCoveredCells != null)
			{
				coveredCells = new List<CellField>(directionsCoveredCells.Count);
				foreach (Directions direction in directionsCoveredCells)
				{

					switch (direction)
					{
						case Directions.bottom:
							coveredCells.Add(content[X - 1, Y]);
							break;
						case Directions.left:
							coveredCells.Add(content[X, Y - 1]);
							break;
						case Directions.left_bottom:
							coveredCells.Add(content[X - 1, Y - 1]);
							break;
						case Directions.left_top:
							coveredCells.Add(content[X + 1, Y - 1]);
							break;
						case Directions.right:
							coveredCells.Add(content[X, Y + 1]);
							break;
						case Directions.right_bottom:
							coveredCells.Add(content[X - 1, Y + 1]);
							break;
						case Directions.right_top:
							coveredCells.Add(content[X + 1, Y + 1]);
							break;
						case Directions.top:
							coveredCells.Add(content[X + 1, Y]);
							break;
					}
				}
				coveredCells.Add(content[X, Y]);
			}
			else
			{
			    coveredCells = new List<CellField>() { content[X, Y] };
			}
			return coveredCells;
		}

		/// <summary>
		/// Распологает объект с карты
		/// </summary>
		public void Put(IFieldObject fieldObject)
        {
			//определяем центральную клетку, на которых находиться данный объект
			int X = (int)Math.Truncate(fieldObject.Location.Item1 / lengthOfSideCell);
			int Y = (int)Math.Truncate(fieldObject.Location.Item2 / lengthOfSideCell);

			List<CellField> coveredCells = GetCovered(fieldObject);
			fieldObject.CoveredCells = coveredCells;

			foreach (CellField cell in coveredCells)
			{
				cell.OnThisCell.Add(fieldObject);
			}		
		}

		/// <summary>
		/// Удаляет объект с карты
		/// </summary>
        public void Remove(IFieldObject fieldObject)
        {
			//удаляем ссылки на объект с клеток
			foreach (CellField cell in fieldObject.CoveredCells)
			{
				cell.OnThisCell.Remove(fieldObject);
			}
			//обнуляем сслыку на коллекцию "накрытых клеток"
			fieldObject.CoveredCells = null;
		}

		public void Move(IFieldObject fieldObject)
		{
			//определяем новые клетки, на которых находиться данный объект
			var newCoveredCells = GetCovered(fieldObject);
			var coveredCells = fieldObject.CoveredCells.Except(newCoveredCells);
			
				foreach (CellField cell in coveredCells)
				{
					if (cell.OnThisCell.Contains(fieldObject)) cell.OnThisCell.Remove(fieldObject);
					else cell.OnThisCell.Add(fieldObject);
				}
				fieldObject.CoveredCells = newCoveredCells;
		}
	}
}