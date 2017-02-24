namespace MudDesigner.Runtime.Game
{
    public interface ITimeOfDay : ICloneableComponent<ITimeOfDay>
    {
        int Hour { get; }

        int Minute { get; }

        void AddHours(int hours);

        void AddMinutes(int minutes);

        void SubtractHours(int hours);

        void SubtractMinutes(int minutes);
    }
}
