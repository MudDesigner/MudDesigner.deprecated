using System.Threading.Tasks;

namespace MudDesigner.Runtime.Game
{
    public class MudUniverseClock : IUniverseClock
    {
        private const int _minutesPerHour = 60;
        private const int _secondsPerMinute = 60;
        private const int _millisecondsPerSecond = 10000;

        private IStopwatch stopwatch;
        private ITimeOfDayFactory timeOfDayFactory;
        private ISubscription universeTImeRequestSubscription;

        public MudUniverseClock(int hoursPerDay, IStopwatch stopwatch, ITimeOfDayFactory timeOfDayFactory, IMessageBrokerFactory brokerFactory)
        {
            this.MessageBroker = brokerFactory.CreateBroker();

            this.stopwatch = stopwatch;
            this.timeOfDayFactory = timeOfDayFactory;
            this.HoursPerDay = hoursPerDay;

            this.CalendarDayToRealHourRatio = 0.5;

            this.universeTImeRequestSubscription = this.MessageBroker.Subscribe<RequestUniverseTimeMessage>(
                (msg, sub) => this.MessageBroker.Publish(new CurrentUniverseTimeMessage(this.GetCurrentUniverseTime())));
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
            this.universeTImeRequestSubscription.Unsubscribe();

            return Task.CompletedTask;
        }

        public ITimeOfDay GetCurrentUniverseTime()
        {
            int hoursIntoCurrentDay = (int)this.stopwatch.GetHours() % this.HoursPerDay;
            int minutesIntoCurrentHour = (int)this.stopwatch.GetMinutes() % _minutesPerHour;
            int secondsIntoCurrentMinute = (int)this.stopwatch.GetSeconds() % _secondsPerMinute;
            int millisecondsIntoCurrentSecond = (int)this.stopwatch.GetMilliseconds() % _millisecondsPerSecond;

            return this.timeOfDayFactory.Create(
                hoursIntoCurrentDay,
                minutesIntoCurrentHour,
                secondsIntoCurrentMinute,
                millisecondsIntoCurrentSecond);
        }
    }
}
