//-----------------------------------------------------------------------
// <copyright file="IZoneFactory.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine.Game
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides methods for creating an instance of an IZone implementation
    /// </summary>
    public interface IZoneFactory
    {
        /// <summary>
        /// Creates a new uninitialized zone.
        /// </summary>
        /// <param name="name">The name of the zone.</param>
        /// <param name="owner">The realm that owns this zone.</param>
        /// <returns>Returns an uninitialized zone instance</returns>
        Task<IZone> CreateZone(string name, IRealm owner);

        /// <summary>
        /// Creates a new uninitialized zone.
        /// </summary>
        /// <param name="name">The name of the zone.</param>
        /// <param name="owner">The realm that owns this zone.</param>
        /// <param name="weatherStates">The weather states to be made available in the zone.</param>
        /// <param name="weatherUpdateFrequency">The frequency to which the weather should be updated.</param>
        /// <returns>
        /// Returns an uninitialized zone instance
        /// </returns>
        Task<IZone> CreateZone(string name, IRealm owner, IEnumerable<IWeatherState> weatherStates, int weatherUpdateFrequency);

        /// <summary>
        /// Creates a new uninitialized zone.
        /// Each of the rooms added will be initialized prior to adding it to the zone.
        /// </summary>
        /// <param name="name">The name of the zone.</param>
        /// <param name="owner">The realm that owns this zone.</param>
        /// <param name="rooms">A collection of rooms that will be initialized and added to the zone.</param>
        /// <returns>Returns an uninitialized zone instance</returns>
        Task<IZone> CreateZone(string name, IRealm owner, IEnumerable<IRoom> rooms);

        /// <summary>
        /// Creates a new uninitialized zone.
        /// Each of the rooms added will be initialized prior to adding it to the zone.
        /// </summary>
        /// <param name="name">The name of the zone.</param>
        /// <param name="owner">The realm that owns this zone.</param>
        /// <param name="weatherStates">The weather states to be made available in the zone.</param>
        /// <param name="weatherUpdateFrequency">The frequency to which the weather should be updated.</param>
        /// <param name="rooms">A collection of rooms that will be initialized and added to the zone.</param>
        /// <returns>
        /// Returns an uninitialized zone instance
        /// </returns>
        Task<IZone> CreateZone(string name, IRealm owner, IEnumerable<IWeatherState> weatherStates, int weatherUpdateFrequency, IEnumerable<IRoom> rooms);
    }
}
