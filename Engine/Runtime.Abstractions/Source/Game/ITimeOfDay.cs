namespace MudDesigner.Runtime.Game
{
    public interface ITimeOfDay
    {
        int Hour { get; }

        int Minute { get; }

        int Second { get; }

        int Millisecond { get; }
    }
}
