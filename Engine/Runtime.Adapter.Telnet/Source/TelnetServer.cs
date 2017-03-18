using MudDesigner.Runtime.Game;
using MudDesigner.Runtime.Networking;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.ObjectModel;
using System.Linq;

namespace MudDesigner.Runtime.Adapter.Telnet
{
    public class TelnetServer : IServer
    {
        private IServerConfiguration serverConfiguration;
        private IGame game;
        private string serverName;

        private Dictionary<IConnection, IPlayer> connectionToPlayerMap;
        private Dictionary<IPlayer, IConnection> playerToConnectionMap;
        private readonly object connectedClientsLock;
        
        private IPlayerFactory playerFactory;
        private IServerContextFactory serverContextFactory;

        public TelnetServer(IGame game, IServerConfiguration serverConfiguration, IPlayerFactory playerFactory, IServerContextFactory contextFactory)
        {
            this.game = game;
            this.serverConfiguration = serverConfiguration;
            this.playerFactory = playerFactory;
            this.serverContextFactory = contextFactory;

            this.connectedClientsLock = new object();
            this.connectionToPlayerMap = new Dictionary<IConnection, IPlayer>();
            this.playerToConnectionMap = new Dictionary<IPlayer, IConnection>();
            this.serverName = $"{this.game} Telnet Server";
            this.State = ServerState.None;
        }

        public event Action<ServerState> OnStateChanged;

        public IMessageBroker MessageBroker => this.game.MessageBroker;

        public IServerContext ServerContext { get; private set; }

        public IServerConfiguration Configuration => this.serverConfiguration;

        public ServerState State { get; private set; }

        public string Name => this.serverName;

        public string Description => "A Telnet based networking server for the Mud Designer.";

        public Task Configure()
        {
            this.SetState(ServerState.Configuring);
            this.ServerContext = this.serverContextFactory.CreateForServer(this);
            this.MessageBroker.Subscribe<ClientConnectedMessage>(async (connectMessage, subscription) => await this.SetupPlayer(connectMessage.Content));

            this.SetState(ServerState.Configured);
            return Task.CompletedTask;
        }

        public IPlayer[] GetConnectedPlayers() => this.playerToConnectionMap.Keys.ToArray();

        public IConnection GetConnectionForPlayer(IPlayer player)
        {
            IConnection connection = null;
            this.playerToConnectionMap.TryGetValue(player, out connection);
            return connection;
        }

        public async Task Initialize()
        {
            if (this.State != ServerState.Configured)
            {
                throw new InvalidOperationException("You must configure the server before you can initialize it.");
            }

            this.SetState(ServerState.Starting);
            this.MessageBroker.Subscribe<NotifyPlayerMessage>(async (message, subscription) =>
            {
                await this.GetConnectionForPlayer(message.Target)?.SendMessage(message.Content);
            });

            await this.ServerContext.Initialize();
            this.SetState(ServerState.Running);
        }

        public Task Update(IGame game)
        {
            return Task.CompletedTask;
        }

        public async Task Delete()
        {
            this.SetState(ServerState.Stopping);
            await this.ServerContext.Delete();

            this.SetState(ServerState.Stopped);
        }

        private async Task SetupPlayer(IConnection connection)
        {
            IPlayer player = this.playerFactory.CreatePlayer();

            Monitor.Enter(this.connectedClientsLock);
            this.connectionToPlayerMap.Add(connection, player);
            this.playerToConnectionMap.Add(player, connection);
            Monitor.Exit(this.connectedClientsLock);

            this.MessageBroker.Publish(new PlayerInstantiatedMessage(player));

            if (!string.IsNullOrEmpty(this.Configuration.ConnectedMessage))
            {
                await connection.SendMessage(this.Configuration.ConnectedMessage);
                await this.Configuration.OnClientConnectedCommand.Execute(player, this.MessageBroker);
            }
        }

        private void SetState(ServerState state)
        {
            this.State = state;
            this.OnStateChanged?.Invoke(state);
        }
    }
}
