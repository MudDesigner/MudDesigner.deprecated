using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MudDesigner.MudEngine.Actors;

namespace MudDesigner.MudEngine.Commanding
{
    public interface ICommandProcessedEventFactory
    {
        void ProcessCommandForActor(CommandResult state, IPlayer actor);
    }
}
