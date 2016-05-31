namespace MudDesigner.Engine.Game
{
    public class CommandRequestData
    {
        public CommandRequestData(string commandData, IPlayer target)
        {
            this.Target = target;
            this.CommandData = commandData;
        }

        public IPlayer Target { get; }

        public string CommandData { get; }
    }
}
