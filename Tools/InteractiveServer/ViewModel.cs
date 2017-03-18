using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using MudDesigner.Runtime;
using MudDesigner.Runtime.Adapter.Telnet;
using MudDesigner.Runtime.Game;
using MudDesigner.Runtime.Networking;
using Newtonsoft.Json;

namespace InteractiveServer
{
    class ViewModel
    {
        const string _configFilename = "Config.json";

        private IGame game;
        private Task runningGameTask;

        public ViewModel()
        {
            //if (!File.Exists(_configFilename))
            //{
                this.Config = new Configuration();
            //    string serializedConfig = JsonConvert.SerializeObject(this.Config, Formatting.Indented);
            //    File.WriteAllText(_configFilename, serializedConfig);
            //}
            //else
            //{
            //    this.Config = JsonConvert.DeserializeObject<Configuration>(_configFilename);
            //}

            this.StartCommand = new AsyncCommandDelegate(this.Initialize);
            this.StopCommand = new AsyncCommandDelegate(this.StopInteractiveMode);
        }

        public Configuration Config { get; set; }

        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }

        public ObservableCollection<string> ServerMessages { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> ClientMessages { get; } = new ObservableCollection<string>();

        internal async Task Initialize()
        {
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
            var server = this.InitializeServer(game);


            game.UseAdapters(server);
            await game.Configure();
            this.runningGameTask = Task.Run(async () => await game.StartAsync());
        }

        private IServer InitializeServer(IGame game)
        {
            var serverConfig = new ServerConfiguration { ServerContextFactory = new SocketContextFactory(), Port = 20000, };
            var server = new TelnetServer(game, serverConfig, null);
            server.OnStateChanged += this.HandleServerStateChanged;
            server.OnConnectionEstablished += this.ClientConnected;
            return server;
        }

        private void HandleServerStateChanged(ServerState obj)
        {
            switch(obj)
            {
                case ServerState.Starting:
                    this.ServerMessages.Add("Server starting.");
                    break;
                case ServerState.Configuring:
                    this.ServerMessages.Add("Configuring the server.");
                    break;
                case ServerState.Configured:
                    this.ServerMessages.Add("Server configuration completed.");
                    break;
                case ServerState.Running:
                    this.ServerMessages.Add("Server is running.");
                    break;
                case ServerState.Stopping:
                    this.ServerMessages.Add("Stopping the server.");
                    break;
                case ServerState.Stopped:
                    this.ServerMessages.Add("Server has been stopped.");
                    break;
            }
        }

        private void ClientConnected(IConnection obj)
        {
            this.ServerMessages.Add("Client connected.");
        }

        private Task InitializeClient()
        {
            return Task.CompletedTask;
        }

        private async Task StopInteractiveMode()
        {
            await this.StopClient();
            await this.StopServer();
        }

        private Task StopClient()
        {
            return Task.CompletedTask;
        }

        private async Task StopServer()
        {
            await this.game.StopAsync();
            this.game = null;

            // Stopping the game will cause the game task to complete - we wait for that to happen.
            await this.runningGameTask;
            this.runningGameTask = null;
        }
    }
}
