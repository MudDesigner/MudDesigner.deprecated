namespace MudDesigner.Engine.Game
{
    public interface ISecurity
    {
        bool CanRunCommand(IActor actor, IActorCommand command);
    }
}