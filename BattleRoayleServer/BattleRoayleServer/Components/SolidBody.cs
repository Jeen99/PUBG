using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleRoayleServer
{
	public abstract class SolidBody : Component
	{

		protected SolidBody(IGameModel gameModel, GameObject parent, Tuple<double, double> location, byte angle, TypesSolid typesSolid) 
			: base(gameModel, parent)
		{

		}

		public TypesSolid TypeSolid { get; private set;}
		

		/// <summary>
		/// Угол поворота объекта(все объекты могут двигаться только по прямой)
		/// </summary>
		public byte Angle { get; private set; }
		

		/// <summary>
		/// Расположение объекта на игровой карте
		/// </summary>
		public Tuple<double, double> Location { get; private set; }
		
	}

	public enum TypesSolid
	{
		Solid,
		SemiSolid,
		Transparent
	}
}