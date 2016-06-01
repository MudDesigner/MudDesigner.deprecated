//-----------------------------------------------------------------------
// <copyright file="StandardServer.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Adapters.Server
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using MudDesigner.Engine;
    using MudDesigner.Engine.Game;

    /// <summary>
    /// Provides members that can be used to interact with a server
    /// </summary>
    public class StandardServer : AdapterBase<IServerConfiguration>, IServer
    {
        /// <summary>
        /// The game that owns this server
        /// </summary>
        private IGame game;

        /// <summary>
        /// All of the connected players, and their connection managers
        /// </summary>
        private Dictionary<IPlayer, IConnection> playerConnections;

        // TODO: playerConnections and playerSockets should be combined into a single collection of List<ConnectedPlayer>
        /// <summary>
        /// All of the connected players and their sockets.
        /// </summary>
        private Dictionary<IPlayer, Socket> playerSockets;

        /// <summary>
        /// The server socket
        /// </summary>
        private Socket serverSocket;

        /// <summary>
        /// The client timeout timer
        /// </summary>
        private EngineTimer<IAdapter> clientTimeoutTimer;

        /// <summary>
        /// The factory responsible for creating new players upon connection
        /// </summary>
        private IPlayerFactory playerFactory;

        /// <summary>
        /// The factory responsible for creating a connection upon receiving a new socket connection.
        /// </summary>
        private IConnectionFactory<StandardServer> connectionFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardServer"/> class.
        /// </summary>
        /// <param name="playerFactory">The player factory.</param>
        /// <param name="connectionFactory">The connection factory.</param>
        public StandardServer(
            IPlayerFactory playerFactory, 
            IConnectionFactory<StandardServer> connectionFactory) 
        {
            this.playerFactory = playerFactory;
            this.connectionFactory = connectionFactory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardServer"/> class.
        /// </summary>
        /// <param name="playerFactory">The player factory.</param>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="serverConfiguration">The server configuration.</param>
        public StandardServer(
            IPlayerFactory playerFactory,
            IConnectionFactory<StandardServer> connectionFactory,
            IServerConfiguration serverConfiguration) 
            : base(serverConfiguration)
        {
            this.playerFactory = playerFactory;
            this.connectionFactory = connectionFactory;
        }

        /// <summary>
        /// Gets or sets the name of this adapter.
        /// </summary>
        public override string Name => $"Mud Engine {RuntimeInformation.OSDescription} Server Adapter";

        /// <summary>
        /// Gets or sets the owner of the server.
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// Gets the status of the server.
        /// </summary>
        public ServerStatus Status { get; internal set; }

        /// <summary>
        /// Gets the port that the server is running on.
        /// </summary>
        public int RunningPort { get; private set; } = 5001;

        /// <summary>
        /// Configures the server using a given configuration.
        /// </summary>
        /// <param name="configuration">The server configuration used to setup the server.</param>
        public override void Configure(IServerConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration), "You must provide a configuration to the server that is not null.");
            }

            this.Configuration = configuration;
        }

        /// <summary>
        /// Initializes the component.
        /// </summary>
        /// <returns>
        /// Returns an awaitable Task
        /// </returns>
        public override Task Initialize()
        {
            if (this.Configuration == null)
            {
                throw new InvalidAdapterStateException(this, $"The {this.Name} adapter requires a valid {typeof(IServerConfiguration).Name} to be provided to it. Please provide one via the {nameof(this.Configure)}({typeof(IServerConfiguration).Name}) method.");
            }
            else if (this.Status != ServerStatus.Stopped)
            {
                throw new InvalidAdapterStateException(this, $"The {this.Name} adapter has already been initialized.");
            }
            else if (this.Configuration.Port > 0)
            {
                this.RunningPort = this.Configuration.Port;
            }

            this.playerConnections = new Dictionary<IPlayer, IConnection>();
            this.playerSockets = new Dictionary<IPlayer, Socket>();
            this.Status = ServerStatus.Stopped;
            this.clientTimeoutTimer = new EngineTimer<IAdapter>(this);

            return Task.FromResult(0);
        }

        /// <summary>
        /// Starts this adapter and allows it to run.
        /// </summary>
        /// <param name="game">The an instance of an initialized game.</param>
        /// <returns>
        /// Returns an awaitable Task
        /// </returns>
        public override Task Start(IGame game)
        {
            if (this.Status != ServerStatus.Stopped)
            {
                throw new InvalidAdapterStateException(this, $"The {this.Name} adapter requires the server to be stopped before you try and start it again.");
            }
            else if (game == null)
            {
                throw new InvalidAdapterStateException(this, $"The {this.Name} adapter require a valid {typeof(IGame).Name} to be provided to it.");
            }

            this.game = game;
            this.Status = ServerStatus.Starting;

            // Get our server address information
            var serverEndPoint = new IPEndPoint(IPAddress.Any, this.RunningPort);

            // Instance the server socket and bind it to a port.
            this.serverSocket = new Socket(serverEndPoint.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            this.serverSocket.Bind(serverEndPoint);
            this.serverSocket.Listen(this.Configuration.MaxQueuedConnections);

            if (this.RaiseOnStartup())
            {
                return Task.FromResult(0);
            }

            // Start a timer that periodically checks for clients that are no longer connected so we can clean them up.
            var staleConnectionPurgeInterval = TimeSpan.FromSeconds(5).TotalMilliseconds;
            this.clientTimeoutTimer.Start(staleConnectionPurgeInterval, staleConnectionPurgeInterval, 0, this.ReviewClientConnectionStates);

            // Begin listening for connection.
            this.Status = ServerStatus.Running;
            return this.ListenForConnection();
        }

        /// <summary>
        /// Lets this instance know that it is about to go out of scope and disposed.
        /// The instance will perform clean-up of its resources in preperation for deletion.
        /// </summary>
        /// <returns>
        /// Returns an awaitable Task
        /// </returns>
        /// <para>
        /// Informs the component that it is no longer needed, allowing it to perform clean up.
        /// Objects registered to one of the two delete events will be notified of the delete request.
        /// </para>
        public override Task Delete()
        {
            if (this.Configuration.OnServerShutdown != null)
            {
                IServerContext context = new ServerContext(this, this.Configuration, this.serverSocket);
                this.Configuration.OnServerShutdown(context);
                if (context.IsHandled)
                {
                    return Task.FromResult(0);
                }
            }

            this.DisconnectAll();

            this.serverSocket.Blocking = false;
            if (this.serverSocket.Poll(100, SelectMode.SelectWrite))
            {
                this.serverSocket.Shutdown(SocketShutdown.Both);
            }

            this.Status = ServerStatus.Stopped;
            this.PublishMessage(new AdapterDeletedMessage(this));

            return Task.FromResult(0);
        }

        public IConnection[] GetConnections() => this.playerConnections.Select(keyValuePair => keyValuePair.Value).ToArray();

        public IConnection GetConnectionForPlayer(IPlayer player)
        {
            IConnection connection = null;
            this.playerConnections.TryGetValue(player, out connection);

            return connection;
        }

        public override string ToString() => $"{this.game.Name} - Adapter: {this.Name}";

        internal Socket GetSocketForPlayer(IPlayer player)
        {
            if (!this.playerSockets.ContainsKey(player))
            {
                return null;
            }

            return this.playerSockets[player];
        }
        
        private async Task ListenForConnection()
        {            
            Socket clientSocket = await this.serverSocket.AcceptAsync();
            await this.ConnectClient(clientSocket);
        }

        private void Disconnect(IPlayer player)
        {
            if (player == null)
            {
                return;
            }

            if (!this.playerConnections.ContainsKey(player))
            {
                return;
            }

            player.Delete();
        }

        /// <summary>
        /// Disconnects all of the currently connected players.
        /// </summary>
        private void DisconnectAll()
        {
            foreach (KeyValuePair<IPlayer, IConnection> pair in this.playerConnections)
            {
                IPlayer player = pair.Key;

                this.Disconnect(player);
            }

            this.playerConnections.Clear();
        }

        /// <summary>
        /// Handles the connection of a client to the server.
        /// </summary>
        /// <param name="socketState">State of the socket.</param>
        private async Task ConnectClient(Socket clientConnection)
        {
            // Fetch the next incoming connection.
            await this.CreatePlayerConnection(clientConnection);
                Socket newConnection = await this.serverSocket.AcceptAsync();
            await this.ConnectClient(newConnection);
        }

        /// <summary>
        /// Creates the player socket connection wrapper.
        /// </summary>
        /// <param name="clientConnection">The client connection.</param>
        private async Task CreatePlayerConnection(Socket clientConnection)
        {
            // Initialize a new player.
            IPlayer player = this.playerFactory.CreatePlayer();
            player.Deleting += this.PlayerDeleting;

            // await player.Initialize();
            this.PublishMessage(new InfoMessage("Player Connected"));

            // Add the player and it's connection to our collection of sockets
            // this.playerSockets.Add(player, clientConnection);

            // Create the user connection instance and store it for the player.
            // IConnection userConnection = this.connectionFactory.CreateConnection(player, this);
            // this.playerConnections.Add(player, userConnection);

            // await userConnection.Initialize();
        }

        /// <summary>
        /// Handles a Player being deleting.
        /// </summary>
        /// <param name="component">The component.</param>
        /// <returns></returns>
        private Task PlayerDeleting(IGameComponent component)
        {
            IPlayer player = component as IPlayer;
            if (player == null)
            {
                return Task.FromResult(0);
            }

            // Clean up the server references to the player
            this.playerSockets.Remove(player);
            this.playerConnections.Remove(player);

            // Remove our strong reference to the event and publish the deletion
            player.Deleting -= this.PlayerDeleting;
            //this.PublishMessage(new PlayerDeletionMessage(player));

            return Task.FromResult(0);
        }

        /// <summary>
        /// Reviews the client connection states and cleans up orphaned player connections.
        /// </summary>
        /// <param name="adapter">The server adapter.</param>
        /// <param name="timer">The timer running to initiate the review.</param>
        private void ReviewClientConnectionStates(IAdapter adapter, EngineTimer<IAdapter> timer)
        {
            var connectedClients = this.playerConnections.ToArray();
            foreach (KeyValuePair<IPlayer, IConnection> pair in connectedClients)
            {
                IPlayer player = pair.Key;
                IConnection connection = pair.Value;
                if (connection.IsConnectionValid())
                {
                    continue;
                }

                this.PublishMessage(new InfoMessage("Player connection timed out."));
                this.Disconnect(player);
            }
        }

        /// <summary>
        /// Raises the server startup event in the server configuration.
        /// </summary>
        /// <returns></returns>
        private bool RaiseOnStartup()
        {
            var startupContext = new ServerContext(this, this.Configuration, this.serverSocket);
            if (this.Configuration.OnServerStartup != null)
            {
                this.Configuration.OnServerStartup(startupContext);

                // Check and see if our context was replaced.
                if (startupContext.ListeningSocket != this.serverSocket)
                {
                    this.serverSocket = startupContext.ListeningSocket;
                }
            }

            return startupContext.IsHandled;
        }
    }
}
