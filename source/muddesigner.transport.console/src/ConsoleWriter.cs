using System;
using System.Text;
using System.Threading.Tasks;

namespace MudEngine.Transport
{
    public class ConsoleWriter : ITransportWriter
    {
        public Task Flush(byte[] data)
        {
            string content = Encoding.UTF8.GetString(data);
            Console.Write(content);
            return Task.CompletedTask;
        }
    }
}