//-----------------------------------------------------------------------
// <copyright file="IRealm.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine.Game
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides Properties and Methods for creating and maintaining rooms.
    /// </summary>
    public interface IRoom : IGameComponent
    {
        /// <summary>
        /// Occurs when an occupant enters the room. The occupant must enter the room through via the AddActorToRoom or AddActorsToRoom methods
        /// </summary>
        event EventHandler<RoomOccupancyChangedEventArgs> OccupantEntered;

        /// <summary>
        /// Occurs when an occupant leaves the room. The occupant must leave the room through via the RemoveActorFromRoom or RemoveActorsFromRoom methods
        /// </summary>
        event EventHandler<RoomOccupancyChangedEventArgs> OccupantLeft;

        /// <summary>
        /// Gets Zone that owns this Room.
        /// </summary>
        IZone Owner { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is sealed. A sealed room will throw an exception if anything tries to add or remove an actor.
        /// A sealed room prevent actors from entering or leaving it.
        /// </summary>
        bool IsSealed { get; }

        /// <summary>
        /// Gets all of the doorways that this room has connected to it.
        /// </summary>
        /// <returns>Returns an array of doorways</returns>
        IDoorway[] GetDoorwaysForRoom();

        /// <summary>
        /// Gets all of the actors that this room has occupying it.
        /// </summary>
        /// <returns>Returns an array of Actors</returns>
        IActor[] GetActorsInRoom();

        /// <summary>
        /// Gets a subset of all actors in the room that are IMob instances only.
        /// </summary>
        /// <returns>Returns an array of Mobs</returns>
        IMob[] GetMobsInRoom();

        /// <summary>
        /// Adds the supplied actor to the room as an occupant.
        /// </summary>
        /// <param name="actor">The actor being added to the room.</param>
        /// <returns>Returns an awaitable Task</returns>
        Task AddActorToRoom(IActor actor);

        /// <summary>
        /// Adds a collection of actors to this room instance.
        /// </summary>
        /// <param name="actors">The actors to add to the room.</param>
        /// <returns>Returns an awaitable Task</returns>
        Task AddActorsToRoom(IEnumerable<IActor> actors);

        /// <summary>
        /// Removes the given actor from this room instance.
        /// </summary>
        /// <param name="actor">The actor to remove.</param>
        /// <returns>Returns an awaitable Task</returns>
        Task RemoveActorFromRoom(IActor actor);

        /// <summary>
        /// Removes a collection of actors from this room instance.
        /// </summary>
        /// <para>
        /// If an actor in the collection does not exist in the room, it is ignored.
        /// </para>
        /// <param name="actors">The actors to remove.</param>
        /// <returns>Returns an awaitable Task</returns>
        Task RemoveActorsFromRoom(IEnumerable<IActor> actors);

        /// <summary>
        /// Creates a doorway that can be configured and be connected to any room.
        /// </summary>
        /// <param name="travelDirectionToReachDoor">The travel direction an actor must travel in order to reach the door.</param>
        /// <returns>Returns a valid instance of an IDoorway implementation</returns>
        Task<IDoorway> CreateDoorway(ITravelDirection travelDirectionToReachDoor);

        /// <summary>
        /// Adds a given doorway to this room instance.
        /// </summary>
        /// <param name="doorway">The doorway doorway to add.</param>
        /// <returns>Returns an awaitable Task</returns>
        Task AddDoorwayToRoom(IDoorway doorway);

        /// <summary>
        /// Removes the given doorway from this room instance.
        /// </summary>
        /// <param name="doorway">The doorway being removed from the room.</param>
        /// <returns>Returns an awaitable Task</returns>
        Task RemoveDoorwayFromRoom(IDoorway doorway);

        /// <summary>
        /// Removes the doorway associated with the travel direction given.
        /// </summary>
        /// <param name="travelDirection">The travel direction that must be removed from the room.</param>
        /// <returns>Returns an awaitable Task</returns>
        Task RemoveDoorwayFromRoom(ITravelDirection travelDirection);

        /// <summary>
        /// Seals the room so that no other actors may enter and any existing IActors may not leave.
        /// </summary>
        void SealRoom();

        /// <summary>
        /// Unseals the room so that actors may enter and leave as they please.
        /// </summary>
        void UnsealRoom();
    }
}
