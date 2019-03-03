using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoayleServer
{
	 public interface IFieldObject
	 {
		void SendMessage(IComponentMsg msg);
		Tuple<double, double> Location { get; }
		GameObject Parent { get; }
		bool CheckCollision(IFieldObject fieldObject);
		IList<Directions> CheckCovered(Tuple<double, double> XDiapason, Tuple<double, double> YDiapason);
		TypesSolidBody Type { get; }
		IList<CellField> CoveredCells { get; set; }
		TypesSolid TypeSolid { get; }
	}

	public enum TypesSolidBody
	{
		Circle,
		Rectangle
	}
}
