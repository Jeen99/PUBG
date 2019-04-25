using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.CommonElements;

namespace BattleRoyalClient
{
	class Explosion : GameObject
	{
		
		public Explosion(ulong ID, PointF location) : base(ID)
		{
			Shape = new RectangleF(location, new SizeF(12, 12));
		}

		public override string TextureName
		{
			get
			{
				return "Explosion";
			}
		}

		public override TypesGameObject Type { get; protected set; } = TypesGameObject.Indefinitely;
	}
}
