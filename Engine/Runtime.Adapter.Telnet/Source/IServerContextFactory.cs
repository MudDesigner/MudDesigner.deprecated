namespace MudDesigner.Runtime.Adapter.Telnet
{
    public interface IServerContextFactory
    {
        IServerContext CreateForServer(TelnetServer server);
    }
}
