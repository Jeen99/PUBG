using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using CommonLibrary.CommonElements;

namespace CommonLibrary.GameMessages
{
	[Serializable]
	public class MagazinState : Message
	{
		public override int Count { get; }

		public override TypesMessage TypeMessage { get; } = TypesMessage.MagazinState;

		public MagazinState(int countBullet)
		{
			Count = countBullet;
		}
	}
}
