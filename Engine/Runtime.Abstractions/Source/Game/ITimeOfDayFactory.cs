using System;
using System.Collections.Generic;
using System.Text;

namespace MudDesigner.Runtime.Game
{
    public interface ITimeOfDayFactory
    {
        ITimeOfDay Create(int hour);

        ITimeOfDay Create(int hour, int minute);

        ITimeOfDay Create(int hour, int minute, int second);

        ITimeOfDay Create(int hour, int minute, int second, int millisecond);

        ITimeOfDay Create(IUniverseClock universeClock, ITimeOfDay timezoneOffset);
    }
}
