using System;
using MudDesigner.MudEngine.Actors;

namespace MudDesigner.MudEngine.Commanding
{
    public class CommandRequestData
    {
        public CommandRequestData(string commandData, IPlayer target, ICommandProcessedEventFactory commandProcessedFactory)
        {
            this.Target = target;
            this.CommandData = commandData;
            this.CommandProcessorFactory = commandProcessedFactory;
        }

        public IPlayer Target { get; }

        public string CommandData { get; }

        public ICommandProcessedEventFactory CommandProcessorFactory { get; }
    }
}
