namespace MudDesigner.Engine.Game
{
    public interface IAccessControl
    {
        string Name { get; }
        
        string Description { get; }
        
        ISecurity SecurityManager { get; }
    }
}