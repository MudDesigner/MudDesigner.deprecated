using System.Threading.Tasks;

namespace MudDesigner.Runtime
{
    public interface IPersistable
    {
        Task Load();

        Task Save();
    }
}
