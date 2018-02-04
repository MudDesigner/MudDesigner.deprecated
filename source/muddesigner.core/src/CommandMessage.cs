namespace MudEngine
{
    public class CommandMessage : IMessage<string>
    {
        public string[] Arguments { get; }

        public string Content { get; }

        public object GetContent() => this.Content;
    }
}