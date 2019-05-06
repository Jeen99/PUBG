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
	public class HealthyState : Message
	{
		public override float HP { get; }

		public override TypesMessage TypeMessage { get; } = TypesMessage.HealthyState;

		public HealthyState(float hP)
		{ 
			HP = hP;
		}
	}
}
