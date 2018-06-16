using System.Threading.Tasks;

namespace MudEngine
{
    public interface IConfigurable
    {
        /// <summary>
        /// Configures the instance. 
        /// Should be called before the instance is used.
        /// </summary>
        /// <returns>Returns a Task that may be awaited.</returns>
        Task Configure();
    }
}