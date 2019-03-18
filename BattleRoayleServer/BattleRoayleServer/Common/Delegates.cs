using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoayleServer
{
	/// <summary>
	/// делегат для создания события загрузки формы клиента
	/// </summary>
	public delegate void GamerIsLoaded(INetworkClient client);
}
