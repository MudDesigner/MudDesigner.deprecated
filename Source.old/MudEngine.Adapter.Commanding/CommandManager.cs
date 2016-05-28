using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MudDesigner.MudEngine.Actors;
using MudDesigner.MudEngine.MessageBrokering;

namespace MudDesigner.MudEngine.Commanding
{
    public class CommandManager : AdapterBase<ICommandingConfiguration>
    {
        private ConcurrentDictionary<IPlayer, List<PlayerCommandHistoryItem>> playerCommandsPendingCompletion
            = new ConcurrentDictionary<IPlayer, List<PlayerCommandHistoryItem>>();

        private ISubscription commandRequestedSubscription;

        public CommandManager()
        {
        }

        public override string Name { get; } = "Command Manager";

        public IActorCommand[] AvailableCommands { get; private set; }

        public ICommandFactory CommandFactory { get; private set; }

        public IGame Game { get; private set; }

        public override void Configure(ICommandingConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration), $"The {typeof(CommandManager).Name} Type requires an instance of {typeof(ICommandingConfiguration).Name} in order to be configured.");
            }
            
            this.CommandFactory = configuration.CommandFactory;
            this.Configuration = configuration;
        }

        public override Task Delete()
        {
            this.commandRequestedSubscription.Unsubscribe();
            return Task.FromResult(0);
        }

        public override Task Initialize()
        {
            // Nothing to initialize here
            return Task.FromResult(0);
        }

        public override Task Start(IGame game)
        {
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game), $"The {typeof(CommandManager).Name} requires an instance of {typeof(IGame).Name} in order to start.");
            }

            this.Game = game;
            this.commandRequestedSubscription = MessageBrokerFactory.Instance.Subscribe<CommandRequestedMessage>(
                async (msg, sub) => await this.ProcessCommand(msg));

            return Task.FromResult(0);
        }

        public void ExecuteCommand(IActorCommand command)
        {
        }

        private async Task ProcessCommand(CommandRequestedMessage requestedCommand)
        {
            // Graba  refernce to the player and split up the player command input data.
            IPlayer player = requestedCommand.Content.Target;
            string[] commandAndArgs = requestedCommand.Content.CommandData.Split(' ');
            if (commandAndArgs.Length == 0)
            {
                // TODO: Determine how to present "invalid command" back to the player.
                return;
            }

            //Grab first element as the command
            string command = commandAndArgs.First();

            // Check if we command already underway, if so attempt to resume it.
            List<PlayerCommandHistoryItem> existingCommandsStack = null;
            if (this.playerCommandsPendingCompletion.TryGetValue(player, out existingCommandsStack))
            {
                //Instead of grabbing the top stack, iterate through the stack to make sure there isn't a nested command.
                //Start at the last added entry of the list and work our way down until we find a match, if any.
                for (int i = existingCommandsStack.Count - 1; i >= 0; i--)
                {
                    PlayerCommandHistoryItem previousCommandItem = existingCommandsStack[i];

                    // Check if the previous command
                    if (await previousCommandItem.Command.CanProcessCommand(player, command))
                    {
                        //If it can be processed, remove it any any commands made after it. Considered all resolved.
                        existingCommandsStack.RemoveRange(i, existingCommandsStack.Count - i);
                        await this.RunCommand(player, command, previousCommandItem.Command, requestedCommand);
                        return;
                    }
                }
            }

            //If there were no previous commands executed, check to see if it is a new valid command 
            if (!this.CommandFactory.IsCommandAvailable(command))
            {
                // TODO: Determine how to notify player of invalid command.
                return;
            }

            // TODO: Check if we have any elements in the array first.
            IActorCommand potentialCommandToExecute = this.CommandFactory.CreateCommand(commandAndArgs.First());
            if (!(await potentialCommandToExecute.CanProcessCommand(player, command)))
            {
                // TODO: Determine how to notify player that the command can't be executed.
                return;
            }

            await this.RunCommand(player, command, potentialCommandToExecute, requestedCommand);
        }

        private async Task RunCommand(IPlayer player, string command, IActorCommand commandToExecute, CommandRequestedMessage commandMessage)
        {
            //Process command and return it's current state
            CommandResult state = await commandToExecute.ProcessCommand(player, command);

            //Send the command results to the assigned Processor
            commandMessage.Content.CommandProcessorFactory.ProcessCommandForActor(state, player);

            //If completed it doesn't need to be added to the history stack so we can return early.
            if (state.IsCompleted)
            {
                //We don't want to clean up their history here because player still may have other commands
                //in process. Also thie cleanup seems to be more setup for when a player is removed from the game.
                //await this.CleanupPlayerHistory(player);
                return;
            }

            //Create a new history item and add it to the list.
            var historyItem = new PlayerCommandHistoryItem(commandToExecute, commandMessage);
            List<PlayerCommandHistoryItem> commandHistoryStack = null;
            if (this.playerCommandsPendingCompletion.TryGetValue(player, out commandHistoryStack))
            {
                commandHistoryStack.Add(historyItem);
                return;
            }

            //If player isn't already in our list, add them.
            commandHistoryStack = new List<PlayerCommandHistoryItem>();
            this.playerCommandsPendingCompletion.TryAdd(player, commandHistoryStack);
            commandHistoryStack.Add(historyItem);

            //Register for cleanup of players history when event for Player Deletion is triggered.
            player.Deleting += this.CleanupPlayerHistory;
        }

        private Task CleanupPlayerHistory(IGameComponent playerComponent)
        {
            IPlayer player = (IPlayer)playerComponent;
            player.Deleting -= this.CleanupPlayerHistory;
            var historyDictionary = (IDictionary<IPlayer, Stack<PlayerCommandHistoryItem>>)this.playerCommandsPendingCompletion;
            historyDictionary.Remove(player);

            return Task.FromResult(0);
        }
    }
}
