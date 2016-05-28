//-----------------------------------------------------------------------
// <copyright file="IZone.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine.Game
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides members for creating and maintaining zones
    /// </summary>
    public interface IZone : IGameComponent
    {
        /// <summary>
        /// Occurs when the weather in the zone has changed.
        /// </summary>
        event EventHandler<WeatherStateChangedEventArgs> WeatherChanged;

        /// <summary>
        /// Occurs when a zone occupant has entered the zone.
        /// </summary>
        event EventHandler<ZoneOccupancyChangedEventArgs> EnteredZone;

        /// <summary>
        /// Occurs when a zone occupant has left the zone.
        /// </summary>
        event EventHandler<ZoneOccupancyChangedEventArgs> LeftZone;

        /// <summary>
        /// Gets the number of rooms in this zone instance.
        /// </summary>
        int NumberOfRoomsInZone { get; }

        /// <summary>
        /// Gets the current state of the weather in this zone.
        /// </summary>
        IWeatherState CurrentWeather { get; }

        /// <summary>
        /// Gets the frequency for how often the weather in the zone could change.
        /// </summary>
        int WeatherUpdateFrequency { get; }

        /// <summary>
        /// Gets the realm that owns this zone.
        /// </summary>
        IRealm Owner { get; set; }

        /// <summary>
        /// Gets all of the weather states for this zone.
        /// </summary>
        /// <returns>Returns an array of weather states</returns>
        IWeatherState[] GetWeatherStatesForZone();

        /// <summary>
        /// Gets all of the rooms for this zone.
        /// </summary>
        /// <returns>Returns an array of rooms</returns>
        IRoom[] GetRoomsForZone();

        /// <summary>
        /// Creates an uninitialized room.
        /// </summary>
        /// <param name="name">The name of the room.</param>
        /// <returns>Returns an uninitialized room instance</returns>
        Task<IRoom> CreateRoom(string name);

        /// <summary>
        /// Adds the given room to this zone.
        /// </summary>
        /// <param name="room">The room to add.</param>
        /// <returns>Returns an awaitable Task</returns>
        Task AddRoomToZone(IRoom room);

        /// <summary>
        /// Adds a collection of rooms to this zone.
        /// </summary>
        /// <param name="rooms">The rooms to add to the zone.</param>
        /// <returns>Returns an awaitable Task</returns>
        Task AddRoomsToZone(IEnumerable<IRoom> rooms);

        /// <summary>
        /// Removes the given room from this zone.
        /// </summary>
        /// <param name="room">The room to remove.</param>
        /// <returns>Returns an awaitable Task</returns>
        Task RemoveRoomFromZone(IRoom room);

        /// <summary>
        /// Removes a collection of rooms from this zone.
        /// </summary>
        /// <para>
        /// Any of the rooms in the collection that do not exist in the zone are ignored.
        /// </para>
        /// <param name="rooms">The rooms to remove.</param>
        /// <returns>Returns an awaitable Task</returns>
        Task RemoveRoomsFromZone(IEnumerable<IRoom> rooms);

        /// <summary>
        /// Adds the given weather state to the zone, allowing it to have the weather defined applied to it.
        /// </summary>
        /// <param name="weatherState">State of the weather.</param>
        /// <returns>Returns an awaitable Task</returns>
        Task AddWeatherState(IWeatherState weatherState);

        /// <summary>
        /// Removes the given weather state from the zone, preventing the zone from applying it any further.
        /// If the zone is currently using the state, it will be removed but the state will not change until the weather frequency is triggered again.
        /// </summary>
        /// <param name="weatherState">State of the weather.</param>
        /// <returns>Returns an awaitable Task</returns>
        Task RemoveWeatherState(IWeatherState weatherState);

        /// <summary>
        /// Determines whether the given room exists within this zone.
        /// </summary>
        /// <param name="room">The room to look for.</param>
        /// <returns>Returns true if the room exists within the zone</returns>
        bool HasRoomInZone(IRoom room);
    }
}
