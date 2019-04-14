using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Common;

namespace BattleRoyalClient
{
	class DeathZone : GameObject
	{
		private TimeSpan timeToChange;
		public TimeSpan TimeToChange { get => timeToChange; set => timeToChange = value; }

		public DeathZone(ulong ID) : base(ID)
		{
			timeToChange = new TimeSpan();
		}

		public override TypesGameObject Type { get; protected set; } = TypesGameObject.DeathZone;

		
	}
}
