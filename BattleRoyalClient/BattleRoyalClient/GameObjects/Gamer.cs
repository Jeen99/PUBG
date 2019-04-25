using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.CommonElements;

namespace BattleRoyalClient
{
	class Gamer : GameObject
	{
		public TypesWeapon CurrentWeapon { get; set; } = TypesWeapon.Not;

		public override string TextureName
		{
			get
			{
				if (CurrentWeapon == TypesWeapon.Not)
					return Type.ToString();
				else
				{
					return Type.ToString() + CurrentWeapon.ToString();
				}
			}
		}

		public Gamer(ulong ID) : base(ID)
		{
		}

		public Gamer(ulong ID, RectangleF shape, double angle = 0) : base(ID, shape, angle)
		{
		}

		public Gamer(ulong ID, PointF location, SizeF size, double angle = 0) : base(ID, location, size, angle)
		{
		}

		public override TypesGameObject Type { get; protected set; } = TypesGameObject.Player;

		
	}
}
