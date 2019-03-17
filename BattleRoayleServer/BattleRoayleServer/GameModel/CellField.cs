﻿using System;
using System.Collections.Generic;
using System.Drawing;
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
        public RectangleF Shape { get; private set; }

		public CellField(RectangleF shape)
		{
			Shape = shape;
		}

    }
}