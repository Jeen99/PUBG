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

		public void Put(string gameobject)
        {
            throw new NotImplementedException();
        }

        public void Remove(GameObject gameObject)
        {
            throw new NotImplementedException();
        }
	}
}