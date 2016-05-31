namespace MudDesigner.Engine.Game
{
    public class CommandRequestedMessage : MessageBase<CommandRequestData>
    {
        public CommandRequestedMessage(string commandData, IPlayer target)
        {
            base.Content = new CommandRequestData(commandData, target);
        }
    }
}
