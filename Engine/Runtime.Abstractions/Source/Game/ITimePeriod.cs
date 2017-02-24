using System;
using System.Collections.Generic;
using System.Text;

namespace MudDesigner.Runtime.Game
{
    /// <summary>
    /// Defines a period of time in a given day that will automatically be entered into and leave.
    /// </summary>
    /// <remarks>
    /// An example of a Time Period would be having one representing Morning and another representing Afternoon.
    /// Each Time Period would have a different starting time, and the game will naturally move from one period to the next
    /// as the calendar for the location increments.
    /// </remarks>
    public interface ITimePeriod : IDescriptor
    {
        ITimeOfDay CurrentTime { get; }

        ITimeOfDay PeriodStartTime { get; }
    }
}
