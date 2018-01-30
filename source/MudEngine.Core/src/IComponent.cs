using System;
using System.Threading.Tasks;

namespace MudEngine.Core
{
    public interface IComponent : IInitializable
    {
        Guid Id { get; }

        bool IsEnabled { get; }

        DateTime CreatedAt { get; }

        double TimeAlive { get; }

        Task Disable();

        Task Enable();
    }
}