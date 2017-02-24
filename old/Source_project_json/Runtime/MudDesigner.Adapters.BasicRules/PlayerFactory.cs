
using MudDesigner.Engine.Game;

namespace MudDesigner.Adapters.BasicRules
{
    public class PlayerFactory : IPlayerFactory
    {
        public IPlayer CreatePlayer()
        {
            var player = new MudPlayer();

            return player;
        }
    }
}