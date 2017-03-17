using System;
using System.Collections.Generic;
using System.Text;

namespace MudDesigner.Runtime.Game
{
    public class DateTimeFactory : IDateTimeFactory
    {
        public IDate CreateDate(int day, int month, int year) => new MudDate(day, month, year);

        public IDateTime CreateDateTime(ITimeOfDay timeOfDay, IDate date) => new MudDateTime(timeOfDay, date);

        public IDateTime CreateDateTime(ulong elapsedMilliseconds, int hoursPerDay)
        {
            // Date
            int dayOfYear = (int)(((elapsedMilliseconds / 1000) / 60) / 60) / hoursPerDay;

            // TODO: Test that this actually works.
            int monthOfYear = ((int)((elapsedMilliseconds / 1000) / 60 / 60) / hoursPerDay) / ((hoursPerDay * 365) / 12);
            int year = ((int)((elapsedMilliseconds / 1000) / 60) / hoursPerDay) / (hoursPerDay * 365);

            int hoursIntoCurrentDay = ((int)((elapsedMilliseconds * 60) * 60) * hoursPerDay) % hoursPerDay;
            int minutesIntoCurrentHour = (int)((elapsedMilliseconds * 60) * 60) % 60;
            int secondsIntoCurrentMinute = (int)(elapsedMilliseconds * 60) % 60;
            int millisecondsIntoCurrentSecond = (int)elapsedMilliseconds % 1000;

            IDate date = this.CreateDate(dayOfYear, monthOfYear, year);
            ITimeOfDay time = this.CreateTimeOfDay(hoursIntoCurrentDay, minutesIntoCurrentHour, secondsIntoCurrentMinute, millisecondsIntoCurrentSecond);
            return this.CreateDateTime(time, date);
        }

        public ITimeOfDay CreateTimeOfDay(int hour, int minute) => new TimeOfDay(hour, minute);

        public ITimeOfDay CreateTimeOfDay(int hour) => this.CreateTimeOfDay(hour, 0, 0, 0);

        public ITimeOfDay CreateTimeOfDay(int hour, int minute, int second) => this.CreateTimeOfDay(hour, minute, 0, 0);

        public ITimeOfDay CreateTimeOfDay(int hour, int minute, int second, int millisecond)
            => new TimeOfDay(hour, minute, second, millisecond);

        public ITimeOfDay CreateTimeOfDay(IUniverseClock universeClock, ITimeOfDay timezoneOffset)
            => new TimeOfDay(universeClock, timezoneOffset);
    }
}
