using System;
using System.Buffers;
using System.Text;
using System.Threading.Tasks;

namespace MudEngine.Transport
{
    public class ConsoleReader : ITransportReader
    {
        ArrayPool<byte> bufferPool = ArrayPool<byte>.Create();

        public Task<byte[]> Read()
        {
            string input = Console.ReadLine();
            byte[] buffer = Encoding.UTF8.GetBytes(input);
            return Task.FromResult(buffer);
        }
    }
}