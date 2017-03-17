using System.Collections.Generic;

namespace MudDesigner.Runtime.Game
{
    public interface ICalendar : IInitializable
    {
        double GetRealworldToGameWorldRatio();

        ITimeOfDay TimeZoneOffset { get; }

        IEnumerable<ITimePeriod> GetTimePeriods();

        void AddTimePeriod(ITimePeriod timePeriod);

        void RemoveTimePeriod(ITimePeriod timePeriod);

        void ApplyTimeZoneOffset(ITimeOfDay offsetTime);

        IDateTime GetLocalDateTime();

        IDateTime GetUniverseDateTime();
    }
}
