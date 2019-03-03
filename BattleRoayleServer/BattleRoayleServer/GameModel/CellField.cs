using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleRoayleServer
{
    public class CellField
    {

		private IList<IFieldObject> onThisCell;
		/// <summary>
		/// Объекты находищиеся в пределах этой клетки
		/// </summary>
		public IList<IFieldObject> OnThisCell {
			get
			{
			
				if (onThisCell == null)
				{
					onThisCell = new List<IFieldObject>();
				}
				return onThisCell;
			}
		}

		    
        /// <summary>
        /// Расположение левого вернего угла клетки на карте
        /// </summary>
        public Tuple<double, double> Location { get; private set; }

		public CellField(Tuple<double, double> location)
		{
			Location = location;
		}

    }
}