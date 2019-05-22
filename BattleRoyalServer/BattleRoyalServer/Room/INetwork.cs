using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleRoyalServer
{
    public interface INetwork
    {
		Dictionary<ulong, INetworkClient> Clients { get; }
		
		void Start();
		void Stop();
        void Dispose();
    }
}