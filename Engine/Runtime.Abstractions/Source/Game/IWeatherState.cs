namespace MudDesigner.Runtime.Game
{
    public interface IWeatherState : IDescriptor
    {
        double OccurrenceProbability { get; }
    }
}
