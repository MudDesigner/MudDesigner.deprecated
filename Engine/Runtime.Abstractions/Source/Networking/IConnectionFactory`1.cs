using MudDesigner.Runtime.Game;

namespace MudDesigner.Runtime.Networking
{
    public interface IConnectionFactory<TServer> 
        where TServer : class, IServer 
    {
        IPlayerFactory PlayerFactory { get; }

        IConnection CreateConnection(TServer server);
    }
}
