using System;
using System.Threading.Tasks;

namespace MudDesigner.Runtime
{
    public interface IComponent
    {
        Guid Id { get; }

        bool IsEnabled { get; }

        DateTime CreatedOn { get; }

        double TimeAlive { get; }

        void Disable();

        void Enable();

        Task Update(IGame game);
    }
}
