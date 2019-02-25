using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleRoayleServer
{
    public interface INetwork
    {
        IList<INetworkClient> Clients { get; }

        void Start();
        void Dispose();
    }
}