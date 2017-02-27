using System;
using System.Collections.Generic;
using System.Text;

namespace MudDesigner.Runtime.Game
{
    public class TimeOfDayFactory : ITimeOfDayFactory
    {
        public ITimeOfDay Create(int hour, int minute) => new TimeOfDay(hour, minute);

        public ITimeOfDay Create(int hour) => this.Create(hour, 0, 0, 0);

        public ITimeOfDay Create(int hour, int minute, int second) => this.Create(hour, minute, 0, 0);

        public ITimeOfDay Create(int hour, int minute, int second, int millisecond)
            => new TimeOfDay(hour, minute, second, millisecond);

        public ITimeOfDay Create(IUniverseClock universeClock, ITimeOfDay timezoneOffset)
            => new TimeOfDay(universeClock, timezoneOffset);
    }
}
