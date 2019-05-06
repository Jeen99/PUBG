using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.Server;

namespace CSInteraction.Server
{
       public interface IController<T>
       {
			void Hanlder_NewMessage();
			IController<T> GetNewControler(ConnectedClient<T> client);
			void Dispose();
		}   
}
