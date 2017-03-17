using System;
using System.Threading.Tasks;

namespace MudDesigner.Runtime.Game
{
    public class MudUniverseClock : IUniverseClock
    {
        private const int _hoursPerDay = 24;
        private const int _daysPerYear = 365;
        private const int _minutesPerHour = 60;
        private const int _secondsPerMinute = 60;
        private const int _millisecondsPerSecond = 10000;
        private const int _minimumMillisecondsToPublishUpdates = 200;

        private IStopwatch stopwatch;
        private CurrentUniverseTimeMessage timeUpdatedMessage;

        private ulong lastMillisecondCheck;
        private IDateTimeFactory dateTimeFactory;

        public MudUniverseClock(IDateTimeFactory dateTimeFactory, IStopwatch stopwatch, IMessageBrokerFactory brokerFactory)
        {
            this.MessageBroker = brokerFactory.CreateBroker();
            this.stopwatch = stopwatch;
            this.dateTimeFactory = dateTimeFactory;
        }

        public IMessageBroker MessageBroker { get; }

        public Guid Id { get; } = Guid.NewGuid();

        public bool IsEnabled { get; private set; }

        public DateTime CreatedOn { get; } = DateTime.UtcNow;

        public double TimeAlive => this.stopwatch.GetSeconds();

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

        public ulong GetUniverseAgeAsMilliseconds() => this.stopwatch.GetMilliseconds();

        public IDateTime GetUniverseDateTime() => this.dateTimeFactory.CreateDateTime(this.GetUniverseAgeAsMilliseconds(), 24);

        public void Disable() => this.stopwatch.Stop();

        public void Enable() => this.stopwatch.Start();

        public Task Update(IGame game)
        {
            ulong currentMilliseconds = this.stopwatch.GetMilliseconds();
            ulong difference = currentMilliseconds - this.lastMillisecondCheck;

            if (difference >= _minimumMillisecondsToPublishUpdates)
            {
                this.MessageBroker.Publish(new CurrentUniverseTimeMessage(this.GetUniverseDateTime()));
                this.lastMillisecondCheck = currentMilliseconds;
            }

            return Task.CompletedTask;
        }
    }
}
