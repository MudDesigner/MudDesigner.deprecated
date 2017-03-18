namespace MudDesigner.Runtime.Networking
{
    public class NetworkMessageReceived : IMessage<string>
    {
        public NetworkMessageReceived(string message) => this.Content = message;

        public string Content { get; }

        public object GetContent() => this.Content;
    }
}
