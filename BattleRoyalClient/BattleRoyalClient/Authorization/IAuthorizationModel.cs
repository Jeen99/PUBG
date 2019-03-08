using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyalClient
{
	public interface IAuthorizationModel
	{
		string NickName { get; }
		string Password { get; }
		StatesAutorizationModel State { get; }
		event ChangeModel AutorizationModelChange;
	}
}
