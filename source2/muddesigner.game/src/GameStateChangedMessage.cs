namespace MudEngine
{
    public class GameStateChangedMessage : IMessage<IGame>
    {
        public GameStateChangedMessage(IGame newState)
        {
            this.Content = newState;
        }

        public IGame Content { get; }

        public object GetContent() => this.Content;
    }
}