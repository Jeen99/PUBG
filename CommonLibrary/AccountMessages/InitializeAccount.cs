using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.CommonElements;

namespace CommonLibrary.AccountMessages
{
	[Serializable]
	public class DataAccount : Message
	{
		/// <summary>
		/// Количество килов сделннаых игроком
		/// </summary>
		public override long Kills { get; }

		/// <summary>
		/// Количество смертей у игрока
		/// </summary>
		public override long Deaths { get; }

		/// <summary>
		/// Количество битв проведенных игроком
		/// </summary>
		public override long Battles { get; }

		public DataAccount(long kills, long deaths, long battles, TimeSpan gameTime)
		{
			Kills = kills;
			Deaths = deaths;
			Battles = battles;
			Time = gameTime;
		}

		public override TimeSpan Time { get; }

		public override TypesMessage TypeMessage { get; } = TypesMessage.InitializeAccount;

	}
}
