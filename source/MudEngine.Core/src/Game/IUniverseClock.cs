namespace MudEngine.Core.Game
{
    public interface IUniverseClock : IInitializable, IComponent
    {
        //IDateTime GetUniverseDateTime();

        ulong GetUniverseAgeAsMilliseconds();
    }
}