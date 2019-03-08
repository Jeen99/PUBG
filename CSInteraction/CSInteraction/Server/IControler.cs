using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.ProgramMessage;

namespace CSInteraction.Server
{
       public interface IController
       {
        void HanlderNewMessage();
        IController GetNewControler(ServerClient client);
		void Dispose();
    }   
}
