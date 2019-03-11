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
		/// Координаты центра игрового объекта на карте
		/// </summary>
		PointF Location { get; }
		/// <summary>
		/// Ссылка на рродительский объект
		/// </summary>
		GameObject Parent { get; }
		/// <summary>
		/// Проверяет происходит ли наслоение одного объекта на дргой или нет
		/// </summary>
		bool CheckCollision(IFieldObject fieldObject);
		/// <summary>
		/// Возвращает клетки на которые еще заходит данный объект кроме центральной
		/// </summary>
		IList<Directions> CheckCovered(Tuple<float, float> XDiapason, Tuple<float, float> YDiapason);
		/// <summary>
		/// Бывают круглые и прямоугольники
		/// </summary>
		TypesSolidBody Type { get; }
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
