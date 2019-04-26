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

		public DataAccount(long kills, long deaths, long battles, TimeSpan gameTime)
		{
			Kills = kills;
			Deaths = deaths;
			Battles = battles;
			Time = gameTime;
		}

		public TimeSpan Time { get; private set; }

		public string Login => throw new NotImplementedException();

		public string Password => throw new NotImplementedException();

		public bool Result => throw new NotImplementedException();

		public Direction Direction => throw new NotImplementedException();

		public PointF Location => throw new NotImplementedException();

		public float Angle => throw new NotImplementedException();

		public int Count => throw new NotImplementedException();

		public TypesWeapon TypeWeapon => throw new NotImplementedException();

		public float HP => throw new NotImplementedException();

		public float Distance => throw new NotImplementedException();

		public bool StartOrEnd => throw new NotImplementedException();

		public int TimePassed => throw new NotImplementedException();

		public float Damage => throw new NotImplementedException();

		public RectangleF Shape => throw new NotImplementedException();

		public float Radius => throw new NotImplementedException();

		public SizeF Size => throw new NotImplementedException();

		public TypesGameObject TypeGameObject => throw new NotImplementedException();


		public TypesMessage TypeMessage { get; } = TypesMessage.InitializeAccount;

		public ulong ID => throw new NotImplementedException();

		public List<IMessage> InsertCollections => throw new NotImplementedException();
	}
}
