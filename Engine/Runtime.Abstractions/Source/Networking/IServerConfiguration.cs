using System.Net;

namespace MudDesigner.Runtime.Networking
{
    public interface IServerConfiguration
    {
        string Owner { get; set; }

        int Port { get; set; }

        IPAddress HostAddress { get; set; }

        int MaxQueuedConnections { get; set; }

        ISecurityPolicy PasswordPolicy { get; set; }

        int PreferredBufferSize { get; set; }

        string[] MessageOfTheDay { get; set; }

        string ConnectedMessage { get; set; }

        IServerContextFactory ServerContextFactory { get; set; }
    }
}
