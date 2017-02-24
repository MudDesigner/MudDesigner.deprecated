//-----------------------------------------------------------------------
// <copyright file="IWorldFactory.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine.Game
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides methods for creating an instance of an IWorld implementation
    /// </summary>
    public interface IWorldFactory
    {
        /// <summary>
        /// Creates an uninitialized world.
        /// </summary>
        /// <param name="name">The name of the world.</param>
        /// <param name="gameDayToRealWorldHoursRatio">The game day to real world hours ratio.</param>
        /// <param name="hoursPerDay">The number of hours per day.</param>
        /// <returns>Returns an IWorld instance</returns>
        Task<IWorld> CreateWorld(string name, double gameDayToRealWorldHoursRatio, int hoursPerDay);

        /// <summary>
        /// Creates an uninitialized world
        /// Each realm provided will be initialized during the worlds initialization.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="gameDayToRealWorldHoursRatio">The game day to real world hours ratio.</param>
        /// <param name="hoursPerDay">The hours per day.</param>
        /// <param name="realms">The realms being added to the world.</param>
        /// <returns>Returns an IWorld instance</returns>
        Task<IWorld> CreateWorld(string name, double gameDayToRealWorldHoursRatio, int hoursPerDay, IEnumerable<IRealm> realms);

        /// <summary>
        /// Creates an uninitialized world
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="gameDayToRealWorldHoursRatio">The game day to real world hours ratio.</param>
        /// <param name="hoursPerDay">The number of hours per day.</param>
        /// <param name="timePeriods">The time periods available to the world.</param>
        /// <returns>Returns an IWorld instance</returns>
        Task<IWorld> CreateWorld(string name, double gameDayToRealWorldHoursRatio, int hoursPerDay, IEnumerable<ITimePeriod> timePeriods);

        /// <summary>
        /// Creates an uninitialized world
        /// Each realm provided will be initialized during the worlds initialization.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="gameDayToRealWorldHoursRatio">The game day to real world hours ratio.</param>
        /// <param name="hoursPerDay">The number of hours per day.</param>
        /// <param name="timePeriods">The time periods available to the world.</param>
        /// <param name="realms">The realms being added to the world.</param>
        /// <returns>Returns an IWorld instance</returns>
        Task<IWorld> CreateWorld(string name, double gameDayToRealWorldHoursRatio, int hoursPerDay, IEnumerable<ITimePeriod> timePeriods, IEnumerable<IRealm> realms);
    }
}
