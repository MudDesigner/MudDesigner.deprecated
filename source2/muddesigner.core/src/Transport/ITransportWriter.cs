using System.Threading.Tasks;

namespace MudEngine.Transport
{
    public interface ITransportWriter
    {
        Task Flush(byte[] data);
    }
}