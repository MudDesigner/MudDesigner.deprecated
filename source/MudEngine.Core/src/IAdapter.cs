using System.Threading.Tasks;

namespace MudEngine.Core
{
    public interface IAdapter : IDescriptor, IConfigurable, IInitializable
    {
        /// <summary>
        /// Updates the adapter state
        /// </summary>
        /// <returns>Returns a Task that may be awaited</returns>
        Task Update();
    }
}
