using System.Threading.Tasks;

namespace MudEngine.Core
{
    public interface IInitializable
    {
        IMessageBroker MessageBroker { get; }

        Task Initialize();

        Task Delete();
    }
}