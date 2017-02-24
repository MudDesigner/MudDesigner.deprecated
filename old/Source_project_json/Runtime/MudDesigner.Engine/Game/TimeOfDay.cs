//-----------------------------------------------------------------------
// <copyright file="TimeOfDay.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine.Game
{
    using System;

    /// <summary>
    /// Provides a means of representing a specific time of day in hours and minutes.
    /// Methods are provided to adjust the time of day if needed.
    /// </summary>
    public sealed class TimeOfDay : ITimeOfDay
    {
        /// <summary>
        /// A default constant value for the number of hours in a given day
        /// </summary>
        const int _defaultHoursPerDay = 24;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeOfDay"/> class.
        /// </summary>
        /// <para>
        /// Creates an instance with a default time of day of 0:00
        /// </para>
        public TimeOfDay() : this(0, 0, _defaultHoursPerDay)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeOfDay"/> class.
        /// </summary>
        /// <param name="hour">The hour to start the time of day at.</param>
        /// <param name="minute">The minute to start the time of day at.</param>
        public TimeOfDay(int hour, int minute) : this(hour, minute, _defaultHoursPerDay)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeOfDay"/> class.
        /// </summary>
        /// <param name="hour">The hour to start the time of day at.</param>
        /// <param name="minute">The minute to start the time of day at.</param>
        /// <param name="hoursPerDay">The number of hours required to complete a full day.</param>
        public TimeOfDay(int hour, int minute, int hoursPerDay)
        {
            this.Hour = hour;
            this.Minute = minute;
            this.HoursPerDay = hoursPerDay;
        }

        /// <summary>
        /// Gets the current hour of the day.
        /// </summary>
        public int Hour { get; private set; }

        /// <summary>
        /// Gets the minute we are currently at within the current hour.
        /// </summary>
        public int Minute { get; private set; }

        /// <summary>
        /// Gets how many hours there are in a single day.
        /// </summary>
        public int HoursPerDay { get; private set; }

        /// <summary>
        /// Increments the time of day by the number of minutes given.
        /// If the value causes the current minutes to become greater than 59, then the current hour is incremented and the remainder minutes to increment are added starting at 0 minutes.
        /// </summary>
        /// <param name="minutes">The minutes to increment.</param>
        public void IncrementByMinute(int minutes)
        {
            if (this.Minute + minutes < 59)
            {
                this.Minute += minutes;
                return;
            }

            // We have to many minutes provided, so we must increase by an hour
            this.IncrementByHour(1);
            int deductedValue = Math.Abs(this.Minute - 59);
            this.Minute = -1;

            // Now that we have increased by an hour, lets continue to increment the minutes.
            if (deductedValue > 0)
            {
                this.IncrementByMinute(minutes - deductedValue);
            }
            else
            {
                this.Minute = 0;
            }
        }

        /// <summary>
        /// Increments the time of day by a the number of hours given.
        /// </summary>
        /// <param name="hours">The hours to add.</param>
        public void IncrementByHour(int hours)
        {
            // We can't increment more than 23 hours. Next hour is rolled over to 0.
            if ((this.Minute == 0 && this.Hour + hours >= this.HoursPerDay) || this.Minute > 0 && this.Hour + hours > (this.HoursPerDay - 1))
            {
                int deductedValue = Math.Abs(this.Hour - this.HoursPerDay);
                this.Hour = 0;

                if (deductedValue > 0)
                {
                    this.IncrementByHour(hours - deductedValue);
                }

                return;
            }

            this.Hour += hours;
        }

        /// <summary>
        /// Decrements the time of day by the number of minutes given.
        /// If the value causes the current minutes to become lower than zero, then the current hour is decremented and the remainder minutes to decrement are deducted starting at 59 minutes.
        /// </summary>
        /// <param name="minutes">The minutes to decrement.</param>
        public void DecrementByMinute(int minutes)
        {
            if (this.Minute - minutes < 0)
            {
                // We can not reduce the number of minutes to less than 0, so we decrement an hour and restart from 59 minutes
                this.DecrementByHour(1);
                int deductedValue = Math.Abs(this.Minute - minutes);

                if (deductedValue > 0)
                {
                    this.Minute = 60;
                }

                // Now that we have increased by an hour, lets continue to increment the minutes.
                this.DecrementByMinute(deductedValue);
            }
            else
            {
                this.Minute -= minutes;
            }
        }

        /// <summary>
        /// Decrements the time of day by a the number of hours given.
        /// </summary>
        /// <param name="hours">The hours to decrement.</param>
        public void DecrementByHour(int hours)
        {
            // We can't decrement less than 0 hours. So we reset to the number of hours in a day.
            if (this.Hour - hours < 0)
            {
                int deductedValue = Math.Abs(this.Hour - hours);
                this.Hour = this.HoursPerDay;

                this.DecrementByHour(deductedValue);
            }
            else
            {
                this.Hour -= hours;
            }
        }

        /// <summary>
        /// Sets the hours per day.
        /// </summary>
        /// <param name="hoursPerDay">The hours per day.</param>
        public void SetHoursPerDay(int hoursPerDay)
        {
            this.HoursPerDay = hoursPerDay;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            string hour = string.Empty;
            string minute = string.Empty;

            hour = this.Hour < 10 ? $"0{this.Hour}" : this.Hour.ToString();
            minute = this.Minute < 10 ? $"0{this.Minute}" : this.Minute.ToString();

            return $"{hour}:{minute}";
        }
        
        public override bool Equals (object obj)
        {
            //
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //
            
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            
            TimeOfDay timeOfDay = (TimeOfDay)obj;
            return timeOfDay.Hour == this.Hour
                && timeOfDay.Minute == this.Minute
                && timeOfDay.HoursPerDay == this.HoursPerDay;
        }
        
        // override object.GetHashCode
        public override int GetHashCode()
        {
            return this.Hour.GetHashCode() ^ this.Minute.GetHashCode() ^ this.HoursPerDay.GetHashCode();
        }

        /// <summary>
        /// Clones the properties of this instance to a new instance.
        /// </summary>
        /// <returns>
        /// Returns a new instance with the properties of this instance copied to it.
        /// </returns>
        /// <para>
        /// Cloning does not guarantee that the internal state of an object will be cloned nor
        /// does it guarantee that the clone will be a deep clone or a shallow.
        /// </para>
        public ITimeOfDay Clone() => new TimeOfDay
        {
            Hour = this.Hour,
            Minute = this.Minute,
            HoursPerDay = this.HoursPerDay
        };
    }
}
