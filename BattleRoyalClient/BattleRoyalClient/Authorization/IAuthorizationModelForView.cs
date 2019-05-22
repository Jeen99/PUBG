using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyalClient
{
	public interface IAuthorizationModerForView
	{
		string NickName { get; }
		string Password { get; }
		bool SaveAutorizationData { get; }
		StatesAutorizationModel State { get; }
		event ChangeAutorizationModel AutorizationModelChange;
	}
}
