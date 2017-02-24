using MudDesigner.Runtime.Networking;

namespace MudDesigner.Runtime.Adapter.Telnet
{
    public class ServerConfiguration : IServerConfiguration
    {
        public string Owner { get; set; }
        public int Port { get; set; }
        public int MaxQueuedConnections { get; set; }
        public ISecurityPolicy PasswordPolicy { get; set; }
        public int PreferredBufferSize { get; set; }
        public string[] MessageOfTheDay { get; set; }
        public string ConnectedMessage { get; set; }
    }
}
