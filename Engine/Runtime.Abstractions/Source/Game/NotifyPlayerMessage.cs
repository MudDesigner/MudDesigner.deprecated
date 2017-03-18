namespace MudDesigner.Runtime.Game
{
    public class NotifyPlayerMessage : IMessage<string>
    {
        public NotifyPlayerMessage(string message, IPlayer player)
        {
            this.Content = message;
            this.Target = player;
        }

        public string Content { get; }

        public IPlayer Target { get; }

        public object GetContent() => this.Content;
    }
}
