using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.CommonElements
{
	[Serializable]
	public enum DirectionHorisontal
	{
		Left,
		Right,
		None
	}

	[Serializable]
	public enum DirectionVertical
	{
		Up,
		Down,
		None
	}

	[Serializable]
	public class Direction
	{
		public DirectionHorisontal Horisontal { get; set; }
		public DirectionVertical Vertical { get; set; }

		public Direction()
		{
			Horisontal = DirectionHorisontal.None;
			Vertical = DirectionVertical.None;
		}

		public Direction(DirectionHorisontal horisontal, DirectionVertical vertical)
		{
			Horisontal = horisontal;
			Vertical = vertical;
		}
	}
}
