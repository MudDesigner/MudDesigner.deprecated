namespace MudDesigner.Runtime.Networking
{
    public class ClientConnectedMessage : IMessage<IConnection>
    {
        public ClientConnectedMessage(IConnection connection) => this.Content = connection;

        public IConnection Content { get; }

        public object GetContent() => this.Content;
    }
}
