using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Common;

namespace BattleRoayleServer
{
	 public interface IFieldObject
	 {
		/// <summary>
		/// Отправляет сообщение данному объекту
		/// </summary>
		void SendMessage(IComponentMsg msg);
		/// <summary>
		/// Описывает форму прямоугольника
		/// </summary>
		RectangleF Shape { get; }
		/// <summary>
		/// Ссылка на рродительский объект
		/// </summary>
		GameObject Parent { get; }	
		/// <summary>
		/// Клекти на которых в данный момент распологается данный объект
		/// </summary>
		IList<CellField> CoveredCells { get; set; }
		/// <summary>
		/// Твердый и прозрачный
		/// </summary>
		TypesSolid TypeSolid { get; }
	}

	public enum TypesSolidBody
	{
		Circle,
		Rectangle
	}
}
