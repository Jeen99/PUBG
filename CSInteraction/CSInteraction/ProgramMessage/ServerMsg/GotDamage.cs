using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSInteraction.ProgramMessage
{

	public class GotDamage : IMessage
	{
		public float Damage { get; private set; }

		public TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.GotDamage;

		public GotDamage(float damage)
		{
			Damage = damage;
		}
	}
}
