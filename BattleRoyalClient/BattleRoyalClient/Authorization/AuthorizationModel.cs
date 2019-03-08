using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyalClient
{
	
	public class AuthorizationModel:IAuthorizationModel
	{
		public string NickName { get; set; }
		public string Password { get; set; }
		public StatesAutorizationModel State { get; set; }
		public void CreateModelChange()
		{
			AutorizationModelChange();
		}
		public event ChangeModel AutorizationModelChange;
	}
	public enum StatesAutorizationModel
	{
		NoAutorization,
		ErrorConnect,
		IncorrectData
	}


}
