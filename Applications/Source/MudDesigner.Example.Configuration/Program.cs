using System;
using System.Threading.Tasks;

namespace MudEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var gameConfiguration = new GameConfiguration
            {
                GameName = "Sample Game",
                GameDescription = "A game to demonstrate how to configure Mud Designer games",
                MessageBrokerFactory = new MessageBrokerFactory(),
            };

            var game = new DefaultGame(gameConfiguration);
            game.Initialize().GetAwaiter().Result;
        }
    }
}
