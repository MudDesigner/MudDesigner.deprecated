using MudDesigner.Runtime.Game;
using MudDesigner.Runtime.Networking;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MudDesigner.Runtime.Adapter.Telnet
{
    public class TelnetServer : IServer
    {
        private IServerConfiguration serverConfiguration;
        private IGame game;
        private string serverName;

        private Dictionary<IConnection, IPlayer> connectedClients;
        private IConnectionFactory<TelnetServer> serverConnectionFactory;
        private IServerContext serverContext;

        public TelnetServer(IGame game, IServerConfiguration serverConfiguration, IConnectionFactory<TelnetServer> connectionFactory)
        {
            this.game = game;
            this.serverConfiguration = serverConfiguration;
            this.serverConnectionFactory = connectionFactory;

            this.serverName = $"{this.game} Telnet Server";
            this.State = ServerState.None;
        }

        public event Action<ServerState> OnStateChanged;

        public event Action<IConnection> OnConnectionEstablished;

        public event Action<IConnection> OnDisconnection;

        public IMessageBroker MessageBroker => this.game.MessageBroker;

        public IServerConfiguration Configuration => this.serverConfiguration;

        public ServerState State { get; private set; }

        public string Name => this.serverName;

        public string Description => "A Telnet based networking server for the Mud Designer.";

        public Task Configure()
        {
            this.SetState(ServerState.Configuring);
            this.serverContext = this.Configuration.ServerContextFactory.CreateForServer(this);

            this.SetState(ServerState.Configured);
            return Task.CompletedTask;
        }

        public IEnumerable<IConnection> GetConnections()
        {
            throw new NotImplementedException();
        }

        public async Task Initialize()
        {
            if (this.State != ServerState.Configured)
            {
                throw new InvalidOperationException("You must configure the server before you can initialize it.");
            }

            this.SetState(ServerState.Starting);
            this.serverContext.MessageReceived += (msg) => this.MessageBroker.Publish(new NetworkMessageReceived(msg));
            await this.serverContext.Initialize();
            this.SetState(ServerState.Running);
        }

        public Task Update(IGame game)
        {
            return Task.CompletedTask;
        }

        public async Task Delete()
        {
            this.SetState(ServerState.Stopping);
            await this.serverContext.Delete();

            this.SetState(ServerState.Stopped);
        }

        private void SetState(ServerState state)
        {
            this.State = state;
            this.OnStateChanged?.Invoke(state);
            //this.MessageBroker?.Publish(new ServerStateChangedMessage(this));
        }
    }
}
