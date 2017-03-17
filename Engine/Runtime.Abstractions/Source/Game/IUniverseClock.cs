namespace MudDesigner.Runtime.Game
{
    public interface IUniverseClock : IInitializable, IComponent
    {
        IDateTime GetUniverseDateTime();

        ulong GetUniverseAgeAsMilliseconds();
    }
}
