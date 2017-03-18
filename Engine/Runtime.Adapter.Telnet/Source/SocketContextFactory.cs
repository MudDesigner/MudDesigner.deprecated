using MudDesigner.Runtime.Networking;

namespace MudDesigner.Runtime.Adapter.Telnet
{
    public class SocketContextFactory : IServerContextFactory
    {
        private IMessageBrokerFactory messageBrokerFactory;

        public SocketContextFactory(IMessageBrokerFactory brokerFactory) => this.messageBrokerFactory = brokerFactory;

        public IServerContext CreateForServer(TelnetServer server) => new ServerSocketContext(server);
    }
}
