using MudDesigner.Engine;
using MudDesigner.Engine.Game;

namespace MudDesigner.Adapters.BasicRules
{
    public class PlayerCreatedMessage : MessageBase<IPlayer>
    {
        public PlayerCreatedMessage(IPlayer player)
        {
            this.Content = player;
        }
    }
}
