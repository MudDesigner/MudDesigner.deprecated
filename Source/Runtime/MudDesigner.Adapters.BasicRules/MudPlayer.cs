using System.Threading.Tasks;
using MudDesigner.Engine.Game;

namespace MudDesigner.Adapters.BasicRules
{
    public class MudPlayer : MudCharacter, IPlayer
    {
        protected override Task Load()
        {
            return Task.FromResult(0);
        }

        protected override Task Unload()
        {
            return Task.FromResult(0);
        }
    }
}
