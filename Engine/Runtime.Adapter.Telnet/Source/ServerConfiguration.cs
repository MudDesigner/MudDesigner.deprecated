using System;
using System.Net;
using MudDesigner.Runtime.Networking;

namespace MudDesigner.Runtime.Adapter.Telnet
{
    public class ServerConfiguration : IServerConfiguration
    {
        public string Owner { get; set; }
        public int Port { get; set; } = 23;
        public int MaxQueuedConnections { get; set; } = 10;
        public ISecurityPolicy PasswordPolicy { get; set; }
        public int PreferredBufferSize { get; set; } = 4096;
        public string[] MessageOfTheDay { get; set; }
        public string ConnectedMessage { get; set; }
        public IPAddress HostAddress { get; set; } = IPAddress.Loopback;
        public IServerContextFactory ServerContextFactory { get; set; } = new SocketContextFactory();
    }
}
