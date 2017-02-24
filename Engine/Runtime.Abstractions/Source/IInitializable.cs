using System.Threading.Tasks;

namespace MudDesigner.Runtime
{
    public interface IInitializable
    {
        Task Initialize();

        Task Delete();
    }
}
