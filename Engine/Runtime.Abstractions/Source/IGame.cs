using System;
using System.Threading.Tasks;

namespace MudDesigner.Runtime
{
    public interface IGame : IDescriptor, IConfigurable, IAdaptable, IMessagingComponent
    {
        event Func<GameState, Task> OnStateChanged;

        IGameConfiguration Configuration { get; }

        GameState State { get; }

        Task StartAsync();

        Task StopAsync();
    }
}
