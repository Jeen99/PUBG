using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSInteraction.Server
{
       public interface IController<T>
       {
        void HanlderNewMessage();
        IController<T> GetNewControler(ServerClient<T> client);
		void Dispose();
    }   
}
