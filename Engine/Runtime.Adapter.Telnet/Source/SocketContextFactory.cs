using MudDesigner.Runtime.Networking;

namespace MudDesigner.Runtime.Adapter.Telnet
{
    public class SocketContextFactory : IServerContextFactory
    {
        public IServerContext CreateForServer(IServer server) => new ServerSocketContext(server);
    }
}
