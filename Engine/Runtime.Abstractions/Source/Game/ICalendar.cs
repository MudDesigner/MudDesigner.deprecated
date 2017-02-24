using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MudDesigner.Runtime.Game
{
    public interface ICalendar : IInitializable
    {
        int HoursPerDay { get; }

        ITimeOfDay TimeZoneOffset { get; }

        ITimePeriod CurrentTimeOfDay { get; }

        /// <summary>
        /// Gets the ratio between how many real-world hours must pass before a single in-game calendar day passes.
        /// </summary>
        double CalendarDayToRealHourRatio { get; }

        double RealTimeToCalendarTimeCorrectionFactor { get; }

        IEnumerable<ITimePeriod> GetTimePeriods();

        Task AddTimePeriod(ITimePeriod timePeriod);

        Task RemoveTimePeriod(ITimePeriod timePeriod);

        Task ApplyTimeZoneOffset(int hour, int minute);

        ITimeOfDay GetLocalTime();

        ITimeOfDay GetUniverseTime();
    }
}
