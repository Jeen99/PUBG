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
	public class GotDamage : Message
	{
		public override float Damage { get; }

		public override ulong ID { get; }

		public override TypesMessage TypeMessage { get; } = TypesMessage.GotDamage;

		public GotDamage(ulong iD, float damage)
		{
			ID = iD;
			Damage = damage;
		}
	}
}
