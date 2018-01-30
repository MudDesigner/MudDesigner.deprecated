using System;
using System.Buffers;
using System.Threading.Tasks;

namespace MudEngine.Transport
{
    public class ConsoleReader : ITransportReader
    {
        ArrayPool<byte> bufferPool = ArrayPool<byte>.Create();

        public Task<byte[]> Read()
        {
            string input = Console.ReadLine();
            byte[] buffer = bufferPool.Rent(input.Length);
            return Task.FromResult(buffer);
        }
    }
}