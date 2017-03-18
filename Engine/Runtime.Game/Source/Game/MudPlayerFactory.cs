namespace MudDesigner.Runtime.Game
{
    public class MudPlayerFactory : IPlayerFactory
    {
        private IMessageBrokerFactory messageBrokerFactory;

        public MudPlayerFactory(IMessageBrokerFactory brokerFactory) => this.messageBrokerFactory = brokerFactory;

        public IPlayer CreatePlayer() => new MudPlayer(this.messageBrokerFactory);
    }
}
