using System;
using System.Collections.Generic;

namespace MudDesigner.Runtime.Networking
{
    public interface IServer : IAdapter, IConfigurable
    {
        event Action<ServerState> OnStateChanged;

        ServerState State { get; }

        IServerConfiguration Configuration { get; }

        IEnumerable<IConnection> GetConnections();
    }
}
