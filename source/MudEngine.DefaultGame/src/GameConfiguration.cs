using MudEngine.Core;
using MudEngine.Core.Game;

namespace MudEngine.DefaultGame
{
    public class GameConfiguration : IDescriptor
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int UpdateCadence { get; set; }

        public IPlayerFactory PlayerFactory { get; private set; }

        public void SetPlayerFactoryType<TFactory>(TFactory PlayerFactory) where TFactory : IPlayerFactory
        {
            this.PlayerFactory = PlayerFactory;
        }
    }
}