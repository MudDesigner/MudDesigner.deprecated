using System;
using System.Threading.Tasks;

namespace MudDesigner.Runtime.Game
{
    public class MudUniverseClock : IUniverseClock
    {
        private const int _minutesPerHour = 60;
        private const int _secondsPerMinute = 60;
        private const int _millisecondsPerSecond = 10000;
        private const int _minimumMillisecondsToPublishUpdates = 200;

        private IStopwatch stopwatch;
        private ITimeOfDayFactory timeOfDayFactory;
        private ISubscription universeTImeRequestSubscription;
        private CurrentUniverseTimeMessage timeUpdatedMessage;

        private long lastMillisecondCheck;

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

        public Guid Id { get; } = Guid.NewGuid();

        public bool IsEnabled { get; private set; }

        public DateTime CreatedOn { get; } = DateTime.UtcNow;

        public double TimeAlive => DateTime.UtcNow.Subtract(this.CreatedOn).TotalSeconds;

        public void SetCalendarDayToRealHourRatio(double ratio) => this.CalendarDayToRealHourRatio = ratio;

        public Task Initialize()
        {
            this.stopwatch.Start();
            this.timeUpdatedMessage = new CurrentUniverseTimeMessage(this.GetCurrentUniverseTime());
            return Task.CompletedTask;
        }

        public Task Delete()
        {
            this.stopwatch.Stop();

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

        public void Disable()
        {
            this.stopwatch.Stop();
        }

        public void Enable()
        {
            this.stopwatch.Start();
        }

        public Task Update(IGame game)
        {
            long currentMilliseconds = this.stopwatch.GetMilliseconds();
            long difference = currentMilliseconds - this.lastMillisecondCheck;
            if (difference >= _minimumMillisecondsToPublishUpdates)
            {
                this.timeUpdatedMessage.SetCurrentTime(this.GetCurrentUniverseTime());
                this.MessageBroker.Publish(this.timeUpdatedMessage);
                this.lastMillisecondCheck = currentMilliseconds;
            }

            return Task.CompletedTask;
        }
    }
}
