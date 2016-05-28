//-----------------------------------------------------------------------
// <copyright file="IWeatherState.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine.Game
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides methods for creating and maintaining worlds
    /// </summary>
    public interface IWorld : IGameComponent, ICloneableComponent<IWorld>
    {
        /// <summary>
        /// Occurs when the time of day has changed for this world.
        /// </summary>
        event EventHandler<TimeOfDayChangedEventArgs> TimeOfDayChanged;

        /// <summary>
        /// Gets the time period manager used to manage the different times of the day a world can be in.
        /// </summary>
        TimePeriodManager TimePeriodManager { get; }

        /// <summary>
        /// Gets the current time of day period for this world.
        /// </summary>
        ITimePeriod CurrentTimeOfDay { get; }

        /// <summary>
        /// Gets how many hours it takes to complete one full day in this world.
        /// </summary>
        int HoursPerDay { get; }

        /// <summary>
        /// Gets the adjustment factor used to convert real-world time to in-game time
        /// </summary>
        double GameTimeAdjustmentFactor { get; }

        /// <summary>
        /// Gets or sets the game day to real hour ratio.
        /// </summary>
        double GameDayToRealHourRatio { get; set; }

        /// <summary>
        /// Gets the available time periods for this world.
        /// </summary>
        /// <returns>Returns an array of time periods</returns>
        ITimePeriod[] GetTimePeriodsForWorld();
        
        /// <summary>
        /// Adds a collection of realms to world, initializing them as they are added.
        /// </summary>
        /// <param name="realms">The realms.</param>
        /// <returns>
        /// Returns an awaitable Task
        /// </returns>
        IRealm[] GetRealmsInWorld();

        /// <summary>
        /// Creates an unintialized instance of a realm.
        /// </summary>
        /// <param name="name">The name of the realm.</param>
        /// <param name="owner">The world that owns this realm.</param>
        /// <returns>Returns an initialized instance of IRealm</returns>
        Task<IRealm> CreateRealm(string name);

        /// <summary>
        /// Adds the realms to world.
        /// </summary>
        /// <param name="realms">The realms.</param>
        /// <returns></returns>
        Task AddRealmsToWorld(IEnumerable<IRealm> realms);

        /// <summary>
        /// Initializes and then adds the given realm to this world instance.
        /// </summary>
        /// <param name="realm">The realm to add.</param>
        /// <returns>Returns an awaitable Task</returns>
        Task AddRealmToWorld(IRealm realm);

        /// <summary>
        /// Removes the given realm from this world instance, deleting the realm in the process.
        /// If it must be reused, you may clone the realm and add the clone to another world.
        /// </summary>
        /// <param name="realm">The realm to remove.</param>
        /// <returns>Returns an awaitable Task</returns>
        Task RemoveRealmFromWorld(IRealm realm);

        /// <summary>
        /// Removes a collection of realms from this world instance.
        /// If any of the realms don't exist in the world, they will be ignored.
        /// The realms will be deleted during the process.
        /// If they must be reused, you may clone the realm and add the clone to another world.
        /// </summary>
        /// <param name="realms">The realms collection.</param>
        /// <returns>Returns an awaitable Task</returns>
        Task RemoveRealmsFromWorld(IEnumerable<IRealm> realms);

        /// <summary>
        /// Adds a time period for a day to world.
        /// </summary>
        /// <param name="timePeriod">The time period being added to the world.</param>
        void AddTimePeriodToWorld(ITimePeriod timePeriod);

        /// <summary>
        /// Removes a time period from the world.
        /// </summary>
        /// <param name="timePeriod">The time period being removed.</param>
        void RemoveTimePeriodFromWorld(ITimePeriod timePeriod);

        /// <summary>
        /// Sets how many hours it takes the world to complete a full day.
        /// </summary>
        /// <param name="hours">The number of hours that defines how long a full day is.</param>
        void SetHoursPerDay(int hours);
    }
}
