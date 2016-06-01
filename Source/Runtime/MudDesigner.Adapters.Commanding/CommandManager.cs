using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MudDesigner.Engine;
using MudDesigner.Engine.Game;

namespace MudDesigner.Adapters.Commanding
{
    public class CommandManager : AdapterBase
    {
        private Dictionary<string, IActorCommand> commandAliasMapping;
        
        private Dictionary<IPlayer, List<IActorCommand>> commandsBeingExecuted;
        
        private IActorCommand[] commands;
        
        IMessageBroker broker;
        
        public CommandManager(IActorCommand[] commands)
        {
            this.commands = commands;
            this.commandAliasMapping = new Dictionary<string, IActorCommand>();
            this.commandsBeingExecuted = new Dictionary<IPlayer, List<IActorCommand>>();
        }
        
        public override string Name => nameof(CommandManager);
        
        public IEnumerable<IActorCommand> GetAllowedCommands(ISecurity security, IActor actor)
        {
            foreach (IActorCommand command in this.commands)
            {
                if (!security.ActorHasAccessControl(actor, command.AccessControlRequired))
                {
                    continue;
                }
                
                yield return command;
            }
        }

        public override void Configure()
        {
        }

        public override Task Delete()
        {
            this.commandAliasMapping = null;
            this.commands = null;
            return Task.FromResult(0);
        }

        public override Task Initialize()
        {
            for(int index = 0; index < this.commands.Length; index++)
            {
                IActorCommand currentCommand = this.commands[index];
                string commandName = currentCommand.RootCommandName.ToLower();
                
                if (this.commandAliasMapping.ContainsKey(commandName))
                {
                    throw new DuplicateCommandAliasException(currentCommand);
                }
                
                this.commandAliasMapping.Add(commandName, currentCommand);
            }
            
            if (this.broker == null)
            {
                this.broker = MessageBrokerFactory.Instance;
            }
            
            return Task.FromResult(0);
        }

        public override Task Start(IGame game)
        {
            this.broker.Subscribe<CommandRequestedMessage>(this.HandleIncomingCommandRequest);
            return Task.FromResult(0);
        }

        private async void HandleIncomingCommandRequest(CommandRequestedMessage request, ISubscription subscription)
        {
            string[] requestData = request.Content.CommandData.Split(' ');
            if (requestData.Length == 0)
            {
                // If no message is given, such as the user just pressed enter, ignore it.
                return;
            }
            
            List<IActorCommand> targetCommandHistory;
            IActorCommand commandToExecute;
            if (this.commandsBeingExecuted.TryGetValue(request.Content.Target, out targetCommandHistory))
            {
                commandToExecute = targetCommandHistory.Last();
                if (await commandToExecute.CanProcessCommand(null, request.Content.Target, requestData))
                {
                    await commandToExecute.ProcessCommand(request.Content.Target, requestData);
                }
                return;
            }
            else
            {
                commandToExecute = this.commandAliasMapping[requestData[0].ToLower()];
            }
        }
    }
}