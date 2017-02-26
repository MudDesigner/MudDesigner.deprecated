using System.Threading.Tasks;

namespace MudDesigner.Runtime
{
    public interface IInitializable
    {
        IMessageBroker MessageBroker { get; }

        Task Initialize();

        Task Delete();
    }
}
