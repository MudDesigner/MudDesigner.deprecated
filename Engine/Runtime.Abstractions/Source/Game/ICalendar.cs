using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MudDesigner.Runtime.Game
{
    public interface ICalendar : IInitializable
    {
        ITimeOfDay TimeZoneOffset { get; }

        IEnumerable<ITimePeriod> GetTimePeriods();

        void AddTimePeriod(ITimePeriod timePeriod);

        void RemoveTimePeriod(ITimePeriod timePeriod);

        void ApplyTimeZoneOffset(ITimeOfDay offsetTime);

        ITimeOfDay GetLocalTime();

        IDateTime GetLocalDateTime(); 

        ITimeOfDay GetUniverseTime();

        IDateTime GetUniverseDateTime();
    }
}
