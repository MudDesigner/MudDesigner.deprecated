namespace MudDesigner.Runtime
{
    public class GameStateChangedMessage : MessageBase<IGame>
    {
        public GameStateChangedMessage(IGame game)
        {
            this.Content = game;
        }
    }
}
