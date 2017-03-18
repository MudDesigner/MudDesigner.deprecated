using System;
using System.Collections.Generic;
using MudDesigner.Runtime.Game;

namespace MudDesigner.Runtime.Networking
{
    public interface IServer : IAdapter, IConfigurable
    {
        event Action<ServerState> OnStateChanged;

        ServerState State { get; }

        IServerConfiguration Configuration { get; }

        IPlayer[] GetConnectedPlayers();

        IConnection GetConnectionForPlayer(IPlayer player);
    }
}
