using MudDesigner.Runtime.Networking;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MudDesigner.Runtime.Adapter.Telnet
{
    public class TelnetServer : IServer
    {
        private ServerConfiguration serverConfiguration;
        private IGame game;

        public TelnetServer(IGame game, ServerConfiguration serverConfiguration)
        {
            this.game = game;
            this.serverConfiguration = serverConfiguration;
        }

        public IServerConfiguration Configuration => this.serverConfiguration;

        public ServerState State => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public string Description => throw new NotImplementedException();

        public event Action<ServerState> OnStateChanged;

        public Task Configure()
        {
            this.OnStateChanged?.Invoke(ServerState.Configuring);

            this.OnStateChanged?.Invoke(ServerState.Configured);

            return Task.CompletedTask;
        }

        public IEnumerable<IConnection> GetConnections()
        {
            throw new NotImplementedException();
        }

        public Task Run(IGame game)
        {
            throw new NotImplementedException();
        }

        public Task Update(IGame game)
        {
            throw new NotImplementedException();
        }
    }
}
