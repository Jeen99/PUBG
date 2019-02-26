using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSInteraction.ProgramMessage;

namespace CSInteraction.Server
{
       public interface IControler
       {
        void HanlderNewMessage(IMessage msg);
        IControler GetNewControler(ServerClient client);
    }   
}
