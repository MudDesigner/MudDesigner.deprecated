using MudDesigner.Engine;
using MudDesigner.Engine.Game;

namespace MudDesigner.Adapters.BasicRules
{
    public class PlayerDeletionMessage: MessageBase<IPlayer>
    {
        public PlayerDeletionMessage(IPlayer player)
        {
            this.Content = player;
        }
    }
}