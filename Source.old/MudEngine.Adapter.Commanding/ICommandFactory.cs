namespace MudDesigner.MudEngine.Commanding
{
    public interface ICommandFactory
    {
        IActorCommand CreateCommand(string command);

        bool IsCommandAvailable(string command);
    }
}
