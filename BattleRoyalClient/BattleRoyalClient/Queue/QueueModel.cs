using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyalClient
{
    class QueueModel : IQueueModel
	{
		public string NickName { get; private set; }
		public string Password { get; private set; }
		public int PlaysersInQueue { get; set; }

		public event ChangeModel QueueModelChange;

		public QueueModel(string nickName, string password)
		{
			NickName = nickName;
			Password = password;
			PlaysersInQueue = 0;
		}

		public void CreateChangeModel()
		{
			QueueModelChange();
		}
	}
}
