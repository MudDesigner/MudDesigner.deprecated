using System;
using System.Collections.Generic;
using System.Text;

namespace MudDesigner.Runtime.Networking
{
    public interface IServerContextFactory
    {
        IServerContext CreateForServer(IServer server);
    }
}
