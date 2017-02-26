using MudDesigner.Runtime;
using MudDesigner.Runtime.Adapter.Telnet;
using MudDesigner.Runtime.Networking;
using System;
using System.Threading.Tasks;

namespace MudDesigner.Tools.TelnetServerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Server");
            Run().Wait();
        }

        static async Task Run()
        {


            // Setup the game
            var gameConfig = new MudGameConfiguration
            {
                Name = "Sample Game",
                Description = "Test game to demonstrate the telnet server"
            };
            var game = new MudGame(gameConfig, new SingletonMessageBrokerFactory());

            // Setup the server
            var serverConfig = new ServerConfiguration
            {
                ServerContextFactory = new SocketContextFactory(),
            };
            var server = new TelnetServer(game, serverConfig, null);
            game.UseAdapter(server);
            await game.Configure();
            await game.StartAsync();
        }
    }
}