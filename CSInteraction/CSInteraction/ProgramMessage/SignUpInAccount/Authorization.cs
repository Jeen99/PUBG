using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSInteraction.ProgramMessage
{
	[Serializable]
	public class Authorization: IMessage
	{
		public string Login { get; private set; }
		public string Password { get; private set; }
		public TypesProgramMessage TypeMessage { get; } = TypesProgramMessage.Authorization;

		public Authorization(string login, string password)
		{
			Login = login;
			Password = password;
		}
	}
}
