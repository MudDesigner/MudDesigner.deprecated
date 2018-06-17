using MudEngine.Transport;
using System;
using System.Text;
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

            IMessageBroker broker = gameConfiguration.MessageBrokerFactory.CreateBroker();
            broker.Subscribe<InputReceivedMessage>((msg, sub) =>
            {
                Console.WriteLine($"Message received: {Encoding.UTF8.GetString(msg.Content)}");
                return Task.CompletedTask;
            });

            var game = new DefaultGame(gameConfiguration);
            var consoleAdapter = new ConsoleAdapter(gameConfiguration.MessageBrokerFactory);
            game.UseAdapters(consoleAdapter);
            game.Initialize().GetAwaiter().GetResult();
            Task.Delay(5000).GetAwaiter().GetResult();
            consoleAdapter.WriteMessage("Delay completed").GetAwaiter().GetResult();
        }
    }
}
