using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MudDesigner.Runtime.Game
{
    public class MudCalendar : ICalendar
    {
        private List<ITimePeriod> timePeriods;
        private ITimeOfDayFactory timeOfDayFactory;
        private IUniverseClock universeClock;

        public MudCalendar(IUniverseClock universeClock, IEnumerable<ITimePeriod> timePeriodsForCalendar, IMessageBrokerFactory brokerFactory, ITimeOfDayFactory timeOfDayFactory)
        {
            this.MessageBroker = brokerFactory.CreateBroker();
            this.timeOfDayFactory = timeOfDayFactory;
            this.timePeriods = new List<ITimePeriod>(timePeriodsForCalendar);

            this.universeClock = universeClock;
            this.CalendarDayToRealHourRatio = 0.5;
            this.TimeZoneOffset = this.timeOfDayFactory.Create(0, 0, 0, 0);
            this.HoursPerDay = 24;
        }

        public int HoursPerDay { get; private set; }

        public ITimeOfDay TimeZoneOffset { get; private set; }

        public double CalendarDayToRealHourRatio { get; private set; }

        public double RealTimeToCalendarTimeCorrectionFactor => CalendarDayToRealHourRatio / HoursPerDay;

        public IMessageBroker MessageBroker { get; }

        public void AddTimePeriod(ITimePeriod timePeriod)
        {
            if (this.timePeriods.Contains(timePeriod))
            {
                return;
            }

            this.timePeriods.Add(timePeriod);
        }

        public void RemoveTimePeriod(ITimePeriod timePeriod)
        {
            this.timePeriods.Remove(timePeriod);
        }

        public void ApplyTimeZoneOffset(ITimeOfDay offsetTime)
        {
            this.TimeZoneOffset = offsetTime;
        }

        public Task Initialize()
        {
            return Task.CompletedTask;
        }

        public Task Delete()
        {
            return Task.CompletedTask;
        }

        public IEnumerable<ITimePeriod> GetTimePeriods()
        {
            return this.timePeriods;
        }

        public ITimeOfDay GetLocalTime()
        {
            if (this.TimeZoneOffset == default(ITimeOfDay))
            {
                return this.universeClock.GetCurrentUniverseTime();
            }

            return this.timeOfDayFactory.Create(this.universeClock, this.TimeZoneOffset);
        }

        public ITimeOfDay GetUniverseTime()
        {
            return this.universeClock.GetCurrentUniverseTime();
        }

        public IDateTime GetLocalDateTime()
        {
            throw new NotImplementedException();
        }

        public IDateTime GetUniverseDateTime()
        {
            throw new NotImplementedException();
        }
    }
}
