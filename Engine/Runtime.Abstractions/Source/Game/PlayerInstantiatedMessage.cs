namespace MudDesigner.Runtime.Game
{
    public class PlayerInstantiatedMessage : IMessage<IPlayer>
    {
        public PlayerInstantiatedMessage(IPlayer player) => this.Content = player;

        public IPlayer Content { get; }

        public object GetContent() => this.Content;
    }
}
