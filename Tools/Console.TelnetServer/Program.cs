using MudDesigner.Runtime;
using MudDesigner.Runtime.Adapter.Telnet;
using MudDesigner.Runtime.Game;
using System;
using System.Threading.Tasks;

namespace MudDesigner.Tools.TelnetServerApp
{
    public class TimeManager : IAdapter
    {
        private IUniverseClock clock;

        public TimeManager(IMessageBrokerFactory brokerFactory, IUniverseClock clock)
        {
            this.MessageBroker = brokerFactory.CreateBroker();
            this.clock = clock;
        }

        public string Name => "Time Manager";

        public string Description => "";

        public IMessageBroker MessageBroker { get; }

        public Task Configure()
        {
            return Task.CompletedTask;
        }

        public async Task Delete()
        {
            await this.clock.Delete();
        }

        public async Task Initialize()
        {
            await this.clock.Initialize();
        }

        public async Task Update(IGame game)
        {
            await this.clock.Update(game);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Server");
            Run().Wait();
        }

        static async Task Run()
        {
            IMessageBrokerFactory brokerFactory = new SingletonMessageBrokerFactory();

            // Time Manager setup
            IUniverseClock clock = new MudUniverseClock(new DateTimeFactory(), new OffsetableStopwatch(), brokerFactory);
            var timeManager = new TimeManager(brokerFactory, clock);

            // Setup the game
            var gameConfig = new MudGameConfiguration
            {
                Name = "Sample Game",
                Description = "Test game to demonstrate the telnet server"
            };
            var game = new MudGame(gameConfig, clock, brokerFactory);

            // Setup the server
            var serverConfig = new ServerConfiguration();
            var server = new TelnetServer(game, serverConfig, new MudPlayerFactory(brokerFactory), new SocketContextFactory(brokerFactory));

            IMessageBroker broker = brokerFactory.CreateBroker();
            broker.Subscribe<CurrentUniverseTimeMessage>((msg, sub) => Console.WriteLine(msg.CurrentTime));

            game.UseAdapters(server, timeManager);
            await game.Configure();
            await game.StartAsync();
        }
    }
}