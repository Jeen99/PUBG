using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using CSInteraction.Common;

namespace BattleRoayleServer
{
    public class RoyalField : IField
    {

		public float LengthOfSide
		{
			get
			{
				return lengthOfSide * lengthOfSideCell;
			}
		}
        /// <summary>
        /// Задано статически
        /// </summary>
        private CellField[,] content;
        /// <summary>
        /// Размер стороны карты(карта квадратная)
        /// </summary>
        private const int lengthOfSide = 25;
		/// <summary>
		/// Длина стороны клетки
		/// </summary>
		private const int lengthOfSideCell = 20;

		public RoyalField()
		{
			content = new CellField[lengthOfSide, lengthOfSide];

			for (int i = 0; i < lengthOfSide; i++)
			{
				for (int j = 0; j < lengthOfSide; j++)
				{
					content[i, j] = new CellField(new RectangleF(i * lengthOfSideCell, j * lengthOfSideCell, 
						lengthOfSideCell, lengthOfSideCell));
				}
			}
		}

		/// <summary>
		/// Определяет клетки, которые покрывает данный объект
		/// </summary>
		private List<CellField> GetCovered(IFieldObject fieldObject)
		{
			//определяем центральную клетку, на которых находиться данный объект	
			int X = (int)Math.Floor(fieldObject.Shape.Location.X / lengthOfSideCell);
			int Y = (int)Math.Floor(fieldObject.Shape.Location.Y / lengthOfSideCell);

			if (Y >= lengthOfSide) Y = lengthOfSide - 1;
			if (X >= lengthOfSide) X = lengthOfSide - 1;

			List<CellField> coveredCells = new List<CellField>() { content[X, Y] };

			//определяем на каких клетках находится еще данный объект
			int limit = lengthOfSide - 1;

			if (X <  limit)
				if (content[X + 1 , Y].Shape.IntersectsWith(fieldObject.Shape)) coveredCells.Add(content[X + 1, Y]);
			if (Y < limit)
				if (content[X, Y + 1].Shape.IntersectsWith(fieldObject.Shape)) coveredCells.Add(content[X, Y + 1]);
			if (X < limit && Y < limit)
				if (content[X + 1, Y + 1].Shape.IntersectsWith(fieldObject.Shape)) coveredCells.Add(content[X + 1, Y + 1]);

			return coveredCells;
		}

		/// <summary>
		/// Распологает объект с карты
		/// </summary>
		public void Put(IFieldObject fieldObject)
        {
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
			//получаем клетки с которых нужно удалить объект
			var deleteCells = fieldObject.CoveredCells.Except(newCoveredCells);
			if (deleteCells != null)
			{
				foreach (CellField cell in deleteCells)
				{
					cell.OnThisCell.Remove(fieldObject);
				}
				//получаем клетки с которых нужно удалить объект
			}

			var addCells = newCoveredCells.Except(fieldObject.CoveredCells);
			if (addCells != null)
			{
				foreach (CellField cell in addCells)
				{
					cell.OnThisCell.Add(fieldObject);
				}
			}

			fieldObject.CoveredCells = newCoveredCells;
		}
	}
}