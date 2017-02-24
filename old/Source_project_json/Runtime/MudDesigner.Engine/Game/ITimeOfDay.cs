//-----------------------------------------------------------------------
// <copyright file="ITimeOfDay.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine.Game
{
    // TODO: All ITimeOfDay implementations should be converted from classes to structs
    /// <summary>
    /// Provides properties indicating the current time of day and methods to change the time.
    /// </summary>
    public interface ITimeOfDay : ICloneableComponent<ITimeOfDay>
    {
        /// <summary>
        /// Gets the current hour of the day.
        /// </summary>
        int Hour { get; }

        /// <summary>
        /// Gets how many hours there are in a single day.
        /// </summary>
        int HoursPerDay { get; }

        /// <summary>
        /// Gets the minute we are currently at within the current hour.
        /// </summary>
        int Minute { get; }

        /// <summary>
        /// Decrements the time of day by a the number of hours given.
        /// </summary>
        /// <param name="hours">The hours to decrement.</param>
        void DecrementByHour(int hours);

        /// <summary>
        /// Decrements the time of day by the number of minutes given. 
        /// If the value causes the current minutes to become lower than zero, then the current hour is decremented and the remainder minutes to decrement are deducted starting at 59 minutes.
        /// </summary>
        /// <param name="minutes">The minutes to decrement.</param>
        void DecrementByMinute(int minutes);

        /// <summary>
        /// Increments the time of day by a the number of hours given.
        /// </summary>
        /// <param name="hours">The hours to add.</param>
        void IncrementByHour(int hours);

        /// <summary>
        /// Increments the time of day by the number of minutes given. 
        /// If the value causes the current minutes to become greater than 59, then the current hour is incremented and the remainder minutes to increment are added starting at 0 minutes.
        /// </summary>
        /// <param name="minutes">The minutes to increment.</param>
        void IncrementByMinute(int minutes);

        /// <summary>
        /// Sets the hours per day.
        /// </summary>
        /// <param name="hoursPerDay">The hours per day.</param>
        void SetHoursPerDay(int hoursPerDay);
    }
}