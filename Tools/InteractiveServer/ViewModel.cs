using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using MudDesigner.Runtime;
using MudDesigner.Runtime.Adapter.Telnet;
using MudDesigner.Runtime.Game;
using MudDesigner.Runtime.Networking;

namespace InteractiveServer
{
    public class OnConnectedCommand : IGameCommand
    {
        public string Name => "Connected";

        public string Command => "Connect";

        public ISecurityPolicy Policy => throw new NotImplementedException();

        public async Task Execute(IPlayer target, IMessageBroker broker, params string[] arguments)
        {
            await broker.PublishAsync(new NotifyPlayerMessage("Please enter your name", target));
        }
    }
    class ViewModel
    {
        const string _configFilename = "Config.json";

        private IGame game;
        private Task runningGameTask;
        private Dispatcher mainThread;
        private Socket clientSocket;
        private SocketAsyncEventArgs eventArgs;
        private List<ISubscription> notificationSubscriptions;

        public ViewModel(Dispatcher mainThread)
        {
            this.mainThread = mainThread;
            this.Config = new Configuration();
            this.notificationSubscriptions = new List<ISubscription>();

            this.StartCommand = new AsyncCommandDelegate(this.Initialize);
            this.StopCommand = new AsyncCommandDelegate(this.StopInteractiveMode);
            this.ClientRequestCommand = new CommandDelegate<string>((data) =>
            {
                byte[] buffer = Encoding.UTF8.GetBytes(data);
                var args = new SocketAsyncEventArgs();
                args.SetBuffer(buffer, 0, buffer.Length);
                args.RemoteEndPoint = clientSocket.RemoteEndPoint;
                this.clientSocket.SendAsync(args);
            });
        }

        public Configuration Config { get; set; }

        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }
        public ICommand ClientRequestCommand { get; }

        public ObservableCollection<string> ServerMessages { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> ClientMessages { get; } = new ObservableCollection<string>();

        internal async Task Initialize()
        {
            this.ServerMessages.Clear();
            this.ClientMessages.Clear();

            IMessageBrokerFactory brokerFactory = new SingletonMessageBrokerFactory();

            // Time Manager setup
            IUniverseClock clock = new MudUniverseClock(new DateTimeFactory(), new OffsetableStopwatch(), brokerFactory);
            //var timeManager = new TimeManager(brokerFactory, clock);

            // Setup the game
            var gameConfig = new MudGameConfiguration
            {
                Name = "Sample Game",
                Description = "Test game to demonstrate the telnet server"
            };

            this.game = new MudGame(gameConfig, clock, brokerFactory);
            var server = this.InitializeServer(game, brokerFactory);


            game.UseAdapters(server);
            await game.Configure();
            this.runningGameTask = Task.Run(async () => await game.StartAsync());

            await this.InitializeClient();
        }

        private IServer InitializeServer(IGame game, IMessageBrokerFactory brokerFactory)
        {
            var serverConfig = new ServerConfiguration
            {
                Port = 20000,
                ConnectedMessage = "Welcome to the Interactive Server!",
                OnClientConnectedCommand = new OnConnectedCommand(),
            };

            var server = new TelnetServer(game, serverConfig, new MudPlayerFactory(brokerFactory), new SocketContextFactory(brokerFactory));
            server.OnStateChanged += this.HandleServerStateChanged;

            // Handle subscriptions
            ISubscription subscription = game.MessageBroker.Subscribe<NetworkMessageReceived>(
                (msg, sub) => this.mainThread.Invoke(() => this.ServerMessages.Add($"Client sent: {msg.Content}")),
                (msg) => (string.IsNullOrEmpty(msg.Content) || msg.Content.Equals("\r\n")) ? false : true);

            this.notificationSubscriptions.Add(subscription);

            subscription = game.MessageBroker.Subscribe<PlayerInstantiatedMessage>(
                (msg, sub) => this.mainThread.Invoke(() => this.ServerMessages.Add("New player connection established.")));
            this.notificationSubscriptions.Add(subscription);

            return server;
        }

        private void HandleServerStateChanged(ServerState obj)
        {
            switch (obj)
            {
                case ServerState.Starting:
                    this.mainThread.Invoke(() => this.ServerMessages.Add("Server starting."));
                    break;
                case ServerState.Configuring:
                    this.mainThread.Invoke(() => this.ServerMessages.Add("Configuring the server."));
                    break;
                case ServerState.Configured:
                    this.mainThread.Invoke(() => this.ServerMessages.Add("Server configuration completed."));
                    break;
                case ServerState.Running:
                    this.mainThread.Invoke(() => this.ServerMessages.Add("Server is running."));
                    break;
                case ServerState.Stopping:
                    this.mainThread.Invoke(() => this.ServerMessages.Add("Stopping the server."));
                    break;
                case ServerState.Stopped:
                    this.mainThread.Invoke(() => this.ServerMessages.Add("Server has been stopped."));
                    break;
            }
        }

        private Task InitializeClient()
        {
            var serverEndPoint = new IPEndPoint(IPAddress.Loopback, 20000);
            var clientEndPoint = new IPEndPoint(IPAddress.Any, 20001);
            var clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.eventArgs = new SocketAsyncEventArgs();
            eventArgs.SetBuffer(new byte[256], 0, 256);
            eventArgs.RemoteEndPoint = serverEndPoint;
            eventArgs.Completed += this.ClientNetworkCommunicationCompleted;

            clientSocket.ConnectAsync(eventArgs);
            this.clientSocket = clientSocket;
            return Task.CompletedTask;
        }

        private void ClientNetworkCommunicationCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (e.LastOperation == SocketAsyncOperation.Connect)
            {
                e.ConnectSocket.ReceiveAsync(e);
            }
            else if (e.LastOperation == SocketAsyncOperation.Receive)
            {
                string message = Encoding.UTF8.GetString(e.Buffer, 0, e.BytesTransferred);
                if (string.IsNullOrEmpty(message) || message.Equals("\r\n"))
                {
                    return;
                }

                this.mainThread.Invoke(() => this.ClientMessages.Add(message));
                e.ConnectSocket.ReceiveAsync(e);
            }
        }

        private async Task StopInteractiveMode()
        {
            await this.StopClient();
            await this.StopServer();
        }

        private Task StopClient()
        {
            this.clientSocket.Disconnect(false);
            this.clientSocket.Close();
            this.clientSocket = null;

            this.eventArgs.Dispose();
            this.eventArgs = null;

            return Task.CompletedTask;
        }

        private async Task StopServer()
        {
            foreach (ISubscription subscription in this.notificationSubscriptions)
            {
                subscription.Unsubscribe();
            }

            await this.game.StopAsync();
            this.game = null;

            // Stopping the game will cause the game task to complete - we wait for that to happen.
            await this.runningGameTask;
            this.runningGameTask = null;
        }
    }
}
