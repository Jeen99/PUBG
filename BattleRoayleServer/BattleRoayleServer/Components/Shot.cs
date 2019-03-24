using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSInteraction.ProgramMessage;
using CSInteraction.Common;

namespace BattleRoayleServer
{
	//отвечает за выстрел
	public class Shot:Component
	{

		private Magazin magazin;
		public SolidBody BodyHolder { get; set; }


		public Shot(GameObject parent, Magazin magazin) : base(parent)
		{
			this.magazin = magazin;
		}

		public override void Dispose()
		{
			throw new NotImplementedException();
		}

		public override void UpdateComponent(IMessage msg)
		{
			switch (msg.TypeMessage)
			{
				case TypesProgramMessage.MakeShot:
					Handler_MakeShot(msg as MakeShot);
					break;
			}
		}

		private void Handler_MakeShot(MakeShot msg)
		{

		}

	}
}