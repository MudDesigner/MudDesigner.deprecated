using System.Net.Sockets;

namespace MudDesigner.Runtime.Adapter.Telnet
{
    public interface IServerContext : IInitializable
    {
        Socket ServerSocket { get; }
    }
}
