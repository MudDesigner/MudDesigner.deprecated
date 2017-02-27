namespace MudDesigner.Runtime.Game
{
    public interface IUniverseClock : IInitializable, IComponent
    {
        int HoursPerDay { get; }

        /// <summary>
        /// Gets the ratio between how many real-world hours must pass before a single in-game calendar day passes.
        /// </summary>
        double CalendarDayToRealHourRatio { get; }

        double RealTimeToCalendarTimeCorrectionFactor { get; }

        void SetCalendarDayToRealHourRatio(double ratio);

        ITimeOfDay GetCurrentUniverseTime();
    }
}
