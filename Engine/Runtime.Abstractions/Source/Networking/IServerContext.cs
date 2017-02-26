using System;
using System.Threading.Tasks;

namespace MudDesigner.Runtime.Networking
{
    public interface IServerContext : IInitializable
    {
        event Action<string> MessageReceived;

        Task SendMessage(byte[] message);
    }
}
