using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MudDesigner.MudEngine.Actors;

namespace MudDesigner.MudEngine.Commanding
{
    public class CommandRequestedMessage : MessageBase<CommandRequestData>
    {
        public CommandRequestedMessage(string commandData, IPlayer target, ICommandProcessedEventFactory commandProcessedFactory)
        {
            base.Content = new CommandRequestData(commandData, target, commandProcessedFactory);
        }
    }
}
