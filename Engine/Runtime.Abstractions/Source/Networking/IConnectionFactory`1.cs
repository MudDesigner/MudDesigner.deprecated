namespace MudDesigner.Runtime.Networking
{
    public interface IConnectionFactory<TServer> where TServer : class, IServer
    {
        IConnection CreateConnection(TServer server);
    }
}
