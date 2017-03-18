using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MudDesigner.Runtime.Game;

namespace MudDesigner.Runtime.Networking
{
    public interface IConnection : IInitializable
    {
        event Action<ConnectionClosedArgs> Disconnected;

        bool IsConnectionValid();

        Task SendMessage(string message);

        Task Disconnect();
    }
}
