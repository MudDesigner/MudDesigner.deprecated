using System.Threading.Tasks;

namespace MudEngine.Transport
{
    public interface ITransportReader
    {
        Task<byte[]> Read();
    }
}