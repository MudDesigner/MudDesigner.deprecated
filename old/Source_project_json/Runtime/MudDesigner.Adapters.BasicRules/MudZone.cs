//-----------------------------------------------------------------------
// <copyright file="MudZone.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MudDesigner.Engine;
using MudDesigner.Engine.Game;

namespace MudDesigner.Adapters.BasicRules
{
    public class MudZone : GameComponent, IZone
    {
        /// <summary>
        /// The rooms that this zone holds
        /// </summary>
        List<IRoom> rooms;

        /// <summary>
        /// The weather states that can be applied to this zone
        /// </summary>
        List<IWeatherState> weatherStates;

        /// <summary>
        /// The weather clock
        /// </summary>
        EngineTimer<IWeatherState> weatherClock;

        /// <summary>
        /// The room factory used to create new rooms
        /// </summary>
        IRoomFactory roomFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="MudZone"/> class.
        /// </summary>
        public MudZone(IRoomFactory roomFactory, IRealm owner)
        {
            this.WeatherUpdateFrequency = 15;
            this.rooms = new List<IRoom>();
            this.weatherStates = new List<IWeatherState>();

            this.Owner = owner;
            this.roomFactory = roomFactory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MudZone"/> class.
        /// </summary>
        /// <param name="weatherStates">The weather states that can be applied to this zone.</param>
        public MudZone(IRoomFactory roomFactory, IRealm owner, IEnumerable<IWeatherState> weatherStates) : this(roomFactory, owner)
        {
            this.weatherStates = new List<IWeatherState>(weatherStates);
        }

        /// <summary>
        /// Occurs when the weather in the zone has changed.
        /// </summary>
        public event EventHandler<WeatherStateChangedEventArgs> WeatherChanged;

        /// <summary>
        /// Occurs when a zone occupant has entered the zone.
        /// </summary>
        public event EventHandler<ZoneOccupancyChangedEventArgs> EnteredZone;

        /// <summary>
        /// Occurs when a zone occupant has left the zone.
        /// </summary>
        public event EventHandler<ZoneOccupancyChangedEventArgs> LeftZone;

        /// <summary>
        /// Gets the current state of the weather in this zone.
        /// </summary>
        public IWeatherState CurrentWeather { get; protected set; }

        /// <summary>
        /// Gets the number of rooms in this zone instance.
        /// </summary>
        public int NumberOfRoomsInZone => this.rooms.Count;

        /// <summary>
        /// Gets the realm that owns this zone.
        /// </summary>
        public IRealm Owner { get; set; }

        /// <summary>
        /// Gets the frequency for how often the weather in the zone could change.
        /// </summary>
        public int WeatherUpdateFrequency { get; internal set; }

        /// <summary>
        /// Gets all of the rooms for this zone.
        /// </summary>
        /// <returns>Returns an array of rooms</returns>
        public IRoom[] GetRoomsForZone() => this.rooms.ToArray();

        /// <summary>
        /// Gets all of the weather states for this zone.
        /// </summary>
        /// <returns>Returns an array of weather states</returns>
        public IWeatherState[] GetWeatherStatesForZone() => this.weatherStates.ToArray();

        /// <summary>
        /// Determines whether the given room exists within this zone.
        /// </summary>
        /// <param name="room">The room to look for.</param>
        /// <returns>Returns true if the room exists within the zone</returns>
        public bool HasRoomInZone(IRoom room) => this.rooms.Contains(room);

        /// <summary>
        /// Creates an uninitialized room.
        /// </summary>
        /// <param name="name">The name of the room.</param>
        /// <returns>Returns an uninitialized room instance</returns>
        public Task<IRoom> CreateRoom(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name), "You must provide a valid name when creating a room.");
            }

            return this.roomFactory.CreateRoom(name, this);
        }

        /// <summary>
        /// Adds a collection of rooms to this zone.
        /// </summary>
        /// <param name="rooms">The rooms to add to the zone.</param>
        /// <returns>Returns an awaitable Task</returns>
        /// <exception cref="System.ArgumentNullException">You must not provide a null collection of rooms when attempting to add them to a zone.</exception>
        public async Task AddRoomsToZone(IEnumerable<IRoom> rooms)
        {
            if (rooms == null)
            {
                throw new ArgumentNullException(nameof(rooms), "You must not provide a null collection of rooms when attempting to add them to a zone.");
            }

            foreach(IRoom room in rooms)
            {
                await this.AddRoomToZone(room);
            }
        }

        /// <summary>
        /// Adds the given room to this zone.
        /// </summary>
        /// <param name="room">The room to add.</param>
        /// <returns>
        /// Returns an awaitable Task
        /// </returns>
        public async Task AddRoomToZone(IRoom room)
        {
            if (room == null)
            {
                throw new ArgumentNullException(nameof(room), "You can not add a null room to the zone.");
            }
            else if (string.IsNullOrEmpty(room.Name))
            {
                throw new InvalidRoomException(room, "You must provide the room with a name prior to adding it to the zone.");
            }

            if (this.rooms.Contains(room))
            {
                return;
            }

            await room.Initialize();
            room.OccupantEntered += this.OccupantEnteredRoom;
            room.OccupantLeft += this.OccupantLeftRoom;
            this.rooms.Add(room);
        }

        /// <summary>
        /// Removes the given room from this zone instance, deleting the room in the process.
        /// If it must be reused, you may clone the room and add the clone to another zone.
        /// </summary>
        /// <param name="room">The room to remove.</param>
        /// <returns>Returns an awaitable Task</returns>
        public Task RemoveRoomFromZone(IRoom room)
        {
            if (room == null || !rooms.Contains(room))
            {
                return Task.FromResult(0);
            }

            this.rooms.Remove(room);
            room.OccupantEntered -= this.OccupantEnteredRoom;
            room.OccupantLeft -= this.OccupantLeftRoom;
            return room.Delete();
        }

        /// <summary>
        /// Removes a collection of rooms from this zone instance.
        /// If any of the rooms don't exist in the zone, they will be ignored.
        /// The rooms will be deleted during the process.
        /// If they must be reused, you may clone the rooms and add the clones to another zone.
        /// </summary>
        /// <param name="rooms">The rooms collection.</param>
        /// <returns>Returns an awaitable Task</returns>
        public async Task RemoveRoomsFromZone(IEnumerable<IRoom> rooms)
        {
            if (rooms == null)
            {
                return;
            }

            foreach (IRoom room in rooms)
            {
                await this.RemoveRoomFromZone(room);
            }
        }

        /// <summary>
        /// Adds the given weather state to the zone, allowing it to have the weather defined applied to it.
        /// </summary>
        /// <param name="weatherState">State of the weather.</param>
        /// <returns>Returns an awaitable Task</returns>
        public Task AddWeatherState(IWeatherState weatherState)
        {
            if (weatherState == null)
            {
                throw new ArgumentNullException(nameof(weatherState), "You can not add a null weather state instance to a zone.");
            }
            else if (this.weatherStates.Contains(weatherState))
            {
                return Task.FromResult(0);
            }
            else if (string.IsNullOrEmpty(weatherState.Name))
            {
                throw new InvalidWeatherStateException(weatherState, "You must provide the state with a name before adding it to a zone.");
            }
            else if (weatherState.OccurrenceProbability < 1)
            {
                throw new InvalidWeatherStateException(weatherState, "You must have an occurrence probability set higher than 1.");
            }

            this.weatherStates.Add(weatherState);

            return Task.FromResult(0);
        }

        /// <summary>
        /// Removes the given weather state from the zone, preventing the zone from applying it any further.
        /// If the zone is currently using the state, it will be removed but the state will not change until the weather frequency is triggered again.
        /// </summary>
        /// <param name="weatherState">State of the weather.</param>
        /// <returns>Returns an awaitable Task</returns>
        public Task RemoveWeatherState(IWeatherState weatherState)
        {
            if (weatherState == null || !this.weatherStates.Contains(weatherState))
            {
                return Task.FromResult(0);
            }

            this.weatherStates.Remove(weatherState);
            return Task.FromResult(0);
        }

        /// <summary>
        /// Loads the component and any resources or dependencies it might have.
        /// Called during initialization of the component
        /// </summary>
        /// <returns>Returns an awaitable Task</returns>
        protected override Task Load()
        {
            if (this.weatherStates.Count == 0)
            {
                return Task.FromResult(0);
            }

            this.CurrentWeather = this.weatherStates.AnyOrDefaultFromWeight(state => state.OccurrenceProbability);
            if (this.CurrentWeather == null)
            {
                throw new InvalidZoneException(this, "The zone was not able to initialize with the weather states provided to it.");
            }

            this.weatherClock = new EngineTimer<IWeatherState>(this.CurrentWeather);

            // Start the weather timer, converting the minutes specified 
            // with WeatherUpdateFrequency to in-game minutes using the GameTimeRatio.
            this.weatherClock.Start(
                0,
                TimeSpan.FromMinutes(this.WeatherUpdateFrequency * this.Owner.Owner.GameTimeAdjustmentFactor).TotalMilliseconds,
                0,
                this.SetupWeather);

            return Task.FromResult(0);
        }

        /// <summary>
        /// Unloads this instance and any resources or dependencies it might be using.
        /// Called during deletion of the component.
        /// </summary>
        /// <returns>Returns an awaitable Task</returns>
        protected async override Task Unload()
        {
            this.weatherClock.Dispose();
            foreach(IRoom room in this.rooms)
            {
                room.OccupantEntered -= this.OccupantEnteredRoom;
                room.OccupantLeft -= this.OccupantLeftRoom;
                await room.Delete();
            }
        }

        /// <summary>
        /// Sets up the weather condition in the zone by evaluating the available weather states 
        /// and changing the state if needed.
        /// </summary>
        /// <param name="state">The current weather state.</param>
        /// <param name="timer">The zone timer that is responsible for updating the weather state.</param>
        void SetupWeather(IWeatherState state, EngineTimer<IWeatherState> timer)
        {
            IWeatherState nextState = this.weatherStates.AnyOrDefaultFromWeight(weather => weather.OccurrenceProbability);
            if (nextState == null)
            {
                return;
            }

            var initialState = this.CurrentWeather;
            this.CurrentWeather = nextState;

            this.OnWeatherChanged(initialState, nextState);
            timer.SetState(nextState);
        }

        /// <summary>
        /// Called when the zones weather state has changed
        /// </summary>
        /// <param name="initialState">The state of the weather prior to the change occuring.</param>
        /// <param name="nextState">State of the zones weather now.</param>
        void OnWeatherChanged(IWeatherState initialState, IWeatherState nextState)
        {
            EventHandler<WeatherStateChangedEventArgs> handler = this.WeatherChanged;
            if (handler == null)
            {
                return;
            }

            handler(this, new WeatherStateChangedEventArgs(initialState, nextState));
        }

        /// <summary>
        /// Called when an occupant has entered one of the rooms that this zone owns.
        /// </summary>
        /// <para>
        /// This method will check if the occupant entering the room is entering a room in this zone for the first time.
        /// If so, then the EnteredZone event is raised.
        /// </para>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MudDesigner.MudEngine.Environment.RoomOccupancyChangedEventArgs" /> instance containing the event data.</param>
        void OccupantEnteredRoom(object sender, RoomOccupancyChangedEventArgs e)
        {
            // The occupant is entering our zone for the first time.
            if (e.DepartureRoom == null)
            {
                this.OnEnteredZone(e);
                return;
            }

            // The occupant is moving around within our zone already.
            if (e.DepartureRoom.Owner == this)
            {
                return;
            }

            this.OnEnteredZone(e);
        }

        /// <summary>
        /// Called when an occupant has left one of the rooms that this zone owns.
        /// </summary>
        /// <para>
        /// This method will check if the occupant leaving the room is entering a room in a different zone other than this instance.
        /// If so, then the LeftZone event is raised.
        /// </para>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MudDesigner.MudEngine.Environment.RoomOccupancyChangedEventArgs" /> instance containing the event data.</param>
        void OccupantLeftRoom(object sender, RoomOccupancyChangedEventArgs e)
        {
            // Not sure what happened during travel, but the occupant does not have an arrival room.
            // So we don't do anything or the occupant is just traveling around within our zone, so we ignore it.
            if (e.ArrivalRoom == null || e.ArrivalRoom.Owner == this)
            {
                return;
            }

            this.OnLeftZone(e);
        }

        /// <summary>
        /// Called when an occupant enters a room within this zone for the first time.
        /// </summary>
        /// <param name="roomOccupancyChange">The <see cref="MudDesigner.MudEngine.Environment.RoomOccupancyChangedEventArgs" /> instance containing the event data.</param>
        void OnEnteredZone(RoomOccupancyChangedEventArgs roomOccupancyChange)
        {
            EventHandler<ZoneOccupancyChangedEventArgs> handler = this.EnteredZone;
            if (handler == null)
            {
                return;
            }

            handler(this, new ZoneOccupancyChangedEventArgs(this, roomOccupancyChange));
        }

        /// <summary>
        /// Called when an occupant is leaving a room owned by this zone, and entering a room owned by a different zone.
        /// </summary>
        /// <param name="roomOccupancyChange">The <see cref="MudDesigner.MudEngine.Environment.RoomOccupancyChangedEventArgs" /> instance containing the event data.</param>
        void OnLeftZone(RoomOccupancyChangedEventArgs roomOccupancyChange)
        {
            EventHandler<ZoneOccupancyChangedEventArgs> handler = this.LeftZone;
            if (handler == null)
            {
                return;
            }

            handler(this, new ZoneOccupancyChangedEventArgs(this, roomOccupancyChange));
        }
    }
}
