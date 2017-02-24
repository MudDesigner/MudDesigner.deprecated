using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MudDesigner.MudEngine.Actors;

namespace MudDesigner.MudEngine.Commanding
{
    public interface ICommandProcessedEvent : IMessage
    {
        IActor Target { get; }

        IActorCommand Command { get; }

        CommandResult State { get; }
    }
}
