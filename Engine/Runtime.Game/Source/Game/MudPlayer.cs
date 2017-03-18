namespace MudDesigner.Runtime.Game
{
    public class MudPlayer : IPlayer
    {
        public MudPlayer(IMessageBrokerFactory brokerFactory)
        {
        }

        public IRoom CurrentRoom { get; private set; }
    }
}
