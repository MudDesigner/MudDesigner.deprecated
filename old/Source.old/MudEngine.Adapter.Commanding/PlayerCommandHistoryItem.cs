namespace MudDesigner.MudEngine.Commanding
{
    public class PlayerCommandHistoryItem
    {
        public PlayerCommandHistoryItem(IActorCommand command, CommandRequestedMessage requestedCommandMessage)
        {
            this.RequestedCommand = requestedCommandMessage;
            this.Command = command;
        }

        public CommandRequestedMessage RequestedCommand { get; }

        public IActorCommand Command { get; }
    }
}
