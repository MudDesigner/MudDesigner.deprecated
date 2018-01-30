using System.Text;

namespace MudEngine.Transport
{
    public class InputReceivedMessage : IMessage<byte[]>
    {
        public InputReceivedMessage(byte[] data)
        {
            this.Content=  data;
        }

        public byte[] Content { get; }

        public object GetContent() => this.Content;
    }
}