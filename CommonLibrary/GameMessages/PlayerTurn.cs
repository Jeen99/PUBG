using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.CommonElements;

namespace CommonLibrary.GameMessages
{
	[Serializable]
	public class PlayerTurn : Message
	{
		public override float Angle { get; }

		public override ulong ID { get; }

		public override TypesMessage TypeMessage { get; } = TypesMessage.PlayerTurn;

		public PlayerTurn(ulong iD, float angle)
		{
			ID = iD;
			Angle = angle;
		}
	}
}
