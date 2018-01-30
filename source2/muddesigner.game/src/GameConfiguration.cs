using System;

namespace MudEngine
{
    public class GameConfiguration
    {
        public Guid GameId { get; set; } = Guid.NewGuid();
        public string GameName { get; set; }
        public string GameDescription { get; set; }

        public IMessageBrokerFactory MessageBrokerFactory { get; set; } = new MessageBrokerFactory();
        public int UpdateFrequency { get; set; } = 100;
    }
}