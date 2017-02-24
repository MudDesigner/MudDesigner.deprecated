//-----------------------------------------------------------------------
// <copyright file="TimePeriod.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine.Game
{
    using System;

    /// <summary>
    /// ITimePeriod implementation that handles starting the state
    /// clock and provides methods for resetting and disposing.
    /// </summary>
    public sealed class TimePeriod : ITimePeriod, IDisposable
    {
        /// <summary>
        /// The clock used to track the time of day.
        /// </summary>
        EngineTimer<ITimeOfDay> timeOfDayClock;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimePeriod"/> class.
        /// </summary>
        public TimePeriod()
        {
            this.Id = Guid.NewGuid();
            this.CreationDate = DateTime.Now;
        }

        /// <summary>
        /// Occurs everytime the TimeOfDay is changed.
        /// </summary>
        public event EventHandler<ITimeOfDay> TimeUpdated;

        /// <summary>
        /// Gets or sets the name of this time period.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the start time for this time period. This defines what time of day this period begins at.
        /// </summary>
        public ITimeOfDay StateStartTime { get; set; }

        /// <summary>
        /// Gets the current time of day for this time period.
        /// </summary>
        public ITimeOfDay CurrentTime { get; private set; }

        /// <summary>
        /// Gets the unique identifier for this component.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is enabled.
        /// </summary>
        public bool IsEnabled { get; private set; }

        /// <summary>
        /// Gets the date that this component was instanced.
        /// </summary>
        public DateTime CreationDate { get; }

        /// <summary>
        /// Gets the amount number of seconds that this component instance has been alive.
        /// </summary>
        public double TimeAlive => DateTime.Now.Subtract(this.CreationDate).TotalSeconds;

        /// <summary>
        /// Starts the time period, ticking the time of day clock.
        /// </summary>
        /// <param name="startTime">The time of day this period begins at.</param>
        /// <param name="worldTimeFactor">The world time factor. This value can be used to adjust the interval between time of day updates.</param>
        /// <exception cref="System.ArgumentNullException">startTime can not be null.</exception>
        /// <exception cref="MudDesigner.MudEngine.Environment.InvalidTimeOfDayException">HoursPerDay can not be zero.</exception>
        public void Start(ITimeOfDay startTime, double worldTimeFactor)
        {
            if (startTime == null)
            {
                throw new ArgumentNullException(nameof(startTime), "startTime can not be null.");
            }

            if (startTime.HoursPerDay == 0)
            {
                throw new InvalidTimeOfDayException("HoursPerDay can not be zero.", startTime);
            }

            // Calculate how many seconds in real-world it takes to pass 1 minute in-game.
            double minuteInterval = 60 * worldTimeFactor;

            this.StateStartTime = startTime.Clone();
            this.Reset();

            // Update the state every in-game hour or minute based on the ratio we have
            if (minuteInterval < 0.4)
            {
                this.StartStateClock(TimeSpan.FromSeconds(minuteInterval).TotalMilliseconds, (timeOfDay) => timeOfDay.IncrementByHour(1));
            }
            else
            {
                this.StartStateClock(TimeSpan.FromSeconds(minuteInterval).TotalMilliseconds, (timeOfDay) => timeOfDay.IncrementByMinute(1));
            }

            this.Enable();
        }

        /// <summary>
        /// Resets this instance, zeroing out the current time back to the time periods start time.
        /// </summary>
        public void Reset()
        {
            if (this.timeOfDayClock != null)
            {
                this.timeOfDayClock.Stop();
            }

            this.CurrentTime = this.StateStartTime.Clone();
            this.Disable();
        }

        /// <summary>
        /// Disables this instance.
        /// </summary>
        public void Disable()
        {
            this.IsEnabled = false;
        }

        /// <summary>
        /// Enables this instance.
        /// </summary>
        public void Enable()
        {
            this.IsEnabled = true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Reset();
            this.timeOfDayClock.Dispose();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            if (this.CurrentTime != null)
            {
                return string.Format(
                    "{0} starting at {1}:{2} with a curent time of {3}:{4}",
                    this.Name,
                    this.StateStartTime.Hour,
                    this.StateStartTime.Minute,
                    this.CurrentTime.Hour,
                    this.CurrentTime.Minute);
            }
            else
            {
                return string.Format(
                    "{0} starting at {1}:{2}",
                    this.Name,
                    this.StateStartTime.Hour,
                    this.StateStartTime.Minute);
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            var secondState = (TimePeriod)obj;

            return secondState.StateStartTime.Hour == this.StateStartTime.Hour && secondState.StateStartTime.Minute == this.StateStartTime.Minute;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() => this.StateStartTime.Hour.GetHashCode() * this.StateStartTime.Minute.GetHashCode() * this.Name.GetHashCode();

        /// <summary>
        /// Starts the state clock at the specified interval, firing the callback provided.
        /// </summary>
        /// <param name="interval">The interval.</param>
        /// <param name="callback">The callback.</param>
        void StartStateClock(double interval, Action<ITimeOfDay> callback)
        {
            // If the minute interval is less than 1 second,
            // then we increment by the hour to reduce excess update calls.
            this.timeOfDayClock = new EngineTimer<ITimeOfDay>(this.CurrentTime);
            this.timeOfDayClock.Start(interval, interval, 0, (state, clock) =>
                {
                    callback(state);
                    this.OnTimeUpdated();
                });
        }

        /// <summary>
        /// Called when the states time is updated.
        /// </summary>
        void OnTimeUpdated()
        {
            EventHandler<ITimeOfDay> handler = this.TimeUpdated;
            if (handler == null)
            {
                return;
            }

            handler(this, this.CurrentTime);
        }
    }
}
