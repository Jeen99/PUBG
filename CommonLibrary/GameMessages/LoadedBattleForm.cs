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
	public class LoadedBattleForm : Message
	{

		public override ulong ID { get;  set;}

		public override TypesMessage TypeMessage { get; } = TypesMessage.LoadedBattleForm;

		public LoadedBattleForm()  { }

		public LoadedBattleForm(ulong iD)
		{
			ID = iD;
		}
	}
}
