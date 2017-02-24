//-----------------------------------------------------------------------
// <copyright file="MudZoneFactory.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MudDesigner.Engine.Game;

namespace MudDesigner.Adapters.BasicRules
{
    /// <summary>
    /// Provides methods for creating a new IZone
    /// </summary>
    public sealed class MudZoneFactory : IZoneFactory
    {
        /// <summary>
        /// The room factory
        /// </summary>
        private readonly IRoomFactory roomFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="MudZoneFactory"/> class.
        /// </summary>
        /// <param name="roomFactory">The room factory.</param>
        public MudZoneFactory(IRoomFactory roomFactory)
        {
            this.roomFactory = roomFactory;
        }

        /// <summary>
        /// Creates a new uninitialized zone.
        /// </summary>
        /// <param name="name">The name of the zone.</param>
        /// <param name="owner">The realm that owns this zone.</param>
        /// <returns>Returns an uninitialized zone instance</returns>
        public Task<IZone> CreateZone(string name, IRealm owner)
                    => this.CreateZone(name, owner, Enumerable.Empty<IWeatherState>(), 0, Enumerable.Empty<IRoom>());

        /// <summary>
        /// Creates a new uninitialized zone.
        /// Each of the rooms added will be initialized prior to adding it to the zone.
        /// </summary>
        /// <param name="name">The name of the zone.</param>
        /// <param name="owner">The realm that owns this zone.</param>
        /// <param name="rooms">A collection of rooms that will be initialized and added to the zone.</param>
        /// <returns>Returns an uninitialized zone instance</returns>
        public Task<IZone> CreateZone(string name, IRealm owner, IEnumerable<IRoom> rooms)
                    => this.CreateZone(name, owner, Enumerable.Empty<IWeatherState>(), 0, rooms);

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
        public Task<IZone> CreateZone(string name, IRealm owner, IEnumerable<IWeatherState> weatherStates, int weatherUpdateFrequency)
                    => this.CreateZone(name, owner, weatherStates, weatherUpdateFrequency, Enumerable.Empty<IRoom>());

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
        public async Task<IZone> CreateZone(string name, IRealm owner, IEnumerable<IWeatherState> weatherStates, int weatherUpdateFrequency, IEnumerable<IRoom> rooms)
        {
            MudZone zone = weatherStates.Count() > 0
                ? zone = new MudZone(this.roomFactory, owner, weatherStates)
                : zone = new MudZone(this.roomFactory, owner);

            zone.SetName(name);
            zone.WeatherUpdateFrequency = weatherUpdateFrequency;
            if (rooms.Count() > 0)
            {
                await zone.AddRoomsToZone(rooms);
            }

            return zone;
        }
    }
}
