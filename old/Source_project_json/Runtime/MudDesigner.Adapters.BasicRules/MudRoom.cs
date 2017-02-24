//-----------------------------------------------------------------------
// <copyright file="MudRoom.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MudDesigner.Engine.Game;

namespace MudDesigner.Adapters.BasicRules
{
    public class MudRoom : GameComponent, IRoom
    {
        /// <summary>
        /// The actors currently in the room.
        /// </summary>
        List<IActor> actors;

        /// <summary>
        /// Collection of doors that are connected to this room.
        /// </summary>
        List<IDoorway> doorways;

        /// <summary>
        /// The door factory used to create new doorway instances.
        /// </summary>
        IDoorwayFactory doorFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="MudRoom"/> class.
        /// </summary>
        /// <param name="doorFactory">The door factory used to create new doorway instances.</param>
        public MudRoom(IDoorwayFactory doorFactory, IZone owner)
        {
            this.actors = new List<IActor>();
            this.doorways = new List<IDoorway>();
            this.doorFactory = doorFactory;
            this.IsSealed = true;
            this.Owner = owner;
        }

        /// <summary>
        /// Occurs when an occupant enters the room. The occupant must enter the room through via the AddActorToRoom or AddActorsToRoom methods
        /// </summary>
        public event EventHandler<RoomOccupancyChangedEventArgs> OccupantEntered;

        /// <summary>
        /// Occurs when an occupant leaves the room. The occupant must leave the room through via the RemoveActorFromRoom or RemoveActorsFromRoom methods
        /// </summary>
        public event EventHandler<RoomOccupancyChangedEventArgs> OccupantLeft;

        /// <summary>
        /// Gets a value indicating whether this instance is sealed. A sealed room will throw an exception if anything tries to add or remove an actor.
        /// A sealed room prevent actors from entering or leaving it.
        /// </summary>
        public bool IsSealed { get; protected set; }

        /// <summary>
        /// Gets Zone that owns this Room.
        /// </summary>
        public IZone Owner { get; set; }

        /// <summary>
        /// Gets all of the actors that this room has occupying it.
        /// </summary>
        /// <returns>Returns an array of Actors</returns>
        public IActor[] GetActorsInRoom() => this.actors.ToArray();

        /// <summary>
        /// Gets a subset of all actors in the room that are ICharacter instances only.
        /// </summary>
        /// <returns>Returns an array of Characters</returns>
        public IMob[] GetMobsInRoom() => this.actors.OfType<IMob>().ToArray();

        /// <summary>
        /// Gets all of the doorways that this room has connected to it.
        /// </summary>
        /// <returns>Returns an array of doorways</returns>
        public IDoorway[] GetDoorwaysForRoom() => this.doorways.ToArray();

        /// <summary>
        /// Adds a collection of actors to this room instance.
        /// </summary>
        /// <param name="actors">The actors to add to the room.</param>
        /// <returns>Returns an awaitable Task</returns>
        public async Task AddActorsToRoom(IEnumerable<IActor> actors)
        {
            if (actors == null)
            {
                throw new ArgumentNullException(nameof(actors), "You can not add a null collection of actors to the room");
            }

            foreach(IActor actor in actors)
            {
                await this.AddActorToRoom(actor);
            }
        }

        /// <summary>
        /// Adds the supplied actor to the room as an occupant.
        /// </summary>
        /// <param name="actor">The actor being added to the room.</param>
        /// <returns>Returns an awaitable Task</returns>
        public async Task AddActorToRoom(IActor actor)
        {
            if (actor == null)
            {
                throw new ArgumentNullException(nameof(actor), "You can not add a null actor to the room.");
            }
            else if (string.IsNullOrEmpty(actor.Name))
            {
                throw new InvalidActorException(actor, "You must provide an actor name to the actor you are adding to the room.");
            }

            if (this.actors.Contains(actor))
            {
                return;
            }

            await actor.Initialize();
            this.actors.Add(actor);
        }

        /// <summary>
        /// Adds a given doorway to this room instance.
        /// </summary>
        /// <param name="doorway">The doorway doorway to add.</param>
        /// <returns>Returns an awaitable Task</returns>
        public async Task AddDoorwayToRoom(IDoorway doorway)
        {
            if (doorway.ArrivalRoom == null)
            {
                throw new InvalidRoomException(this, "You can not add a doorway to a room without setting an arrival room for the door.");
            }
            else if (doorway.DepartureRoom == null)
            {
                throw new InvalidRoomException(this, "You can not add a doorway to a room without setting a departure room for the door.");
            }
            else if (doorway.DepartureDirection == null)
            {
                throw new InvalidRoomException(this, "You must set the travel direction required in order to depart this room through the doorway.");
            }

            await doorway.Initialize();
            this.doorways.Add(doorway);
        }

        /// <summary>
        /// Creates a doorway that can be configured and be connected to any room.
        /// </summary>
        /// <param name="travelDirectionToReachDoor">The travel direction an actor must travel in order to reach the door.</param>
        /// <returns>Returns a valid instance of an IDoorway implementation</returns>
        public Task<IDoorway> CreateDoorway(ITravelDirection travelDirectionToReachDoor) 
            => this.doorFactory.CreateDoor($"{this.Name} doorway for the {travelDirectionToReachDoor.ToString()} direction", this, travelDirectionToReachDoor);

        /// <summary>
        /// Removes the given actor from this room instance.
        /// </summary>
        /// <param name="actor">The actor to remove.</param>
        /// <returns>Returns an awaitable Task</returns>
        public Task RemoveActorFromRoom(IActor actor)
        {
            if (actor == null)
            {
                return Task.FromResult(0);
            }
            else if (string.IsNullOrEmpty(actor.Name))
            {
                throw new InvalidActorException(actor, "You can not add an actor to a room without a name.");
            }

            if (this.actors.Contains(actor))
            {
                return Task.FromResult(0);
            }

            this.actors.Remove(actor);
            return Task.FromResult(0);
        }

        /// <summary>
        /// Removes a collection of actors from this room instance.
        /// </summary>
        /// <para>
        /// If an actor in the collection does not exist in the room, it is ignored.
        /// </para>
        /// <param name="actors">The actors to remove.</param>
        /// <returns>Returns an awaitable Task</returns>
        public async Task RemoveActorsFromRoom(IEnumerable<IActor> actors)
        {
            if (actors == null)
            {
                return;
            }

            foreach(IActor actor in actors)
            {
                await this.RemoveActorFromRoom(actor);
            }
        }

        /// <summary>
        /// Removes the doorway associated with the travel direction given.
        /// </summary>
        /// <param name="travelDirection">The travel direction that must be removed from the room.</param>
        /// <returns>Returns an awaitable Task</returns>
        public Task RemoveDoorwayFromRoom(ITravelDirection travelDirection)
        {
            if (travelDirection == null)
            {
                return Task.FromResult(0);
            }

            IDoorway door = this.doorways.FirstOrDefault(d => d.DepartureDirection == travelDirection);
            if (door == null)
            {
                throw new InvalidOperationException($"The {travelDirection.Direction} travel direction does not have a door assigned to it in {this.Name}");
            }

            return this.RemoveDoorwayFromRoom(door);
        }

        /// <summary>
        /// Removes the given doorway from this room instance.
        /// </summary>
        /// <param name="doorway">The doorway being removed from the room.</param>
        /// <returns>Returns an awaitable Task</returns>
        public Task RemoveDoorwayFromRoom(IDoorway doorway)
        {
            if (doorway == null)
            {
                return Task.FromResult(0);
            }

            this.doorways.Remove(doorway);
            return Task.FromResult(0);
        }

        /// <summary>
        /// Seals the room so that no other actors may enter and any existing IActors may not leave.
        /// </summary>
        public void SealRoom()
        {
            this.IsSealed = true;
        }

        /// <summary>
        /// Unseals the room so that actors may enter and leave as they please.
        /// </summary>
        public void UnsealRoom()
        {
            this.IsSealed = false;
        }

        /// <summary>
        /// Loads the component and any resources or dependencies it might have. 
        /// Called during initialization of the component
        /// </summary>
        /// <returns></returns>
        protected override Task Load() => Task.FromResult(0);

        /// <summary>
        /// Unloads this instance and any resources or dependencies it might be using.
        /// Called during deletion of the component.
        /// </summary>
        /// <returns></returns>
        protected override Task Unload() => Task.FromResult(0);
    }
}
