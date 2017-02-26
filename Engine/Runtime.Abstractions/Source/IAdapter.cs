using System.Threading.Tasks;

namespace MudDesigner.Runtime
{
    public interface IAdapter : IDescriptor, IConfigurable, IInitializable
    {
        Task Update(IGame game);
    }
}
