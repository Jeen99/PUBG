using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSInteraction.ProgramMessage
{
	[Serializable]
	public class DataAccount : IMessage
	{
		/// <summary>
		/// Количество килов сделннаых игроком
		/// </summary>
		public long Kills { get; private set;}

		/// <summary>
		/// Количество смертей у игрока
		/// </summary>
		public long Deaths { get; private set; }

		/// <summary>
		/// Количество битв проведенных игроком
		/// </summary>
		public long Battles { get; private set; }

		/// <summary>
		/// Количество времени проведенного в битвах
		/// </summary>
		public DateTime GameTime { get; private set; }

		public DataAccount(long kills, long deaths, long battles, DateTime gameTime)
		{
			Kills = kills;
			Deaths = deaths;
			Battles = battles;
			GameTime = gameTime;
		}

		public TypesProgramMessage TypeMessage { get;} = TypesProgramMessage.DataAccount;
	}
}
