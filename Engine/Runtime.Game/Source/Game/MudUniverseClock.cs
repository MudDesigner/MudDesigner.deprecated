using System.Threading.Tasks;

namespace MudDesigner.Runtime.Game
{
    public class MudUniverseClock : IUniverseClock
    {
        private IStopwatch stopwatch;
        private ITimeOfDayFactory timeOfDayFactory;

        public MudUniverseClock(int hoursPerDay, IStopwatch stopwatch, ITimeOfDayFactory timeOfDayFactory, IMessageBrokerFactory brokerFactory)
        {
            this.MessageBroker = brokerFactory.CreateBroker();

            this.stopwatch = stopwatch;
            this.timeOfDayFactory = timeOfDayFactory;
            this.HoursPerDay = hoursPerDay;

            this.CalendarDayToRealHourRatio = 0.5;
        }

        public IMessageBroker MessageBroker { get; }

        public int HoursPerDay { get; }

        public double CalendarDayToRealHourRatio { get; private set; }

        public double RealTimeToCalendarTimeCorrectionFactor => this.CalendarDayToRealHourRatio / this.HoursPerDay;

        public void SetCalendarDayToRealHourRatio(double ratio) => this.CalendarDayToRealHourRatio = ratio;

        public Task Initialize()
        {
            this.stopwatch.Start();
            return Task.CompletedTask;
        }

        public Task Delete()
        {
            this.stopwatch.Stop();
            return Task.CompletedTask;
        }

        public ITimeOfDay GetCurrentUniverseTime()
        {
            int hoursIntoCurrentDay = this.stopwatch.GetHours() % this.HoursPerDay;
            int minutesIntoCurrentHour = this.stopwatch.GetMinutes();
            int secondsIntoCurrentMinute = this.stopwatch.GetSeconds();
            int millisecondsIntoCurrentSecond = this.stopwatch.GetMilliseconds();

            return this.timeOfDayFactory.Create(
                hoursIntoCurrentDay,
                minutesIntoCurrentHour,
                secondsIntoCurrentMinute,
                millisecondsIntoCurrentSecond);
        }
    }
}
