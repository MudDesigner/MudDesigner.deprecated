using System.Threading.Tasks;

namespace MudEngine
{
    public interface IInitializable
    {
        IMessageBroker MessageBroker { get; }

        Task Initialize();

        Task Delete();
    }
}