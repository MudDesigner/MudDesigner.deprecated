using System.Threading.Tasks;

namespace MudDesigner.Runtime
{
    public interface IAdapter : IDescriptor, IConfigurable
    {
        Task Run(IGame game);

        Task Update(IGame game);
    }
}
