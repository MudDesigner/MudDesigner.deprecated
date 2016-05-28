//-----------------------------------------------------------------------
// <copyright file="IRoomFactory.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine.Game
{
    using System.Threading.Tasks;

    /// <summary>
    /// Provides methods for creating an instance of an IRoom implementation
    /// </summary>
    public interface IRoomFactory
    {
        /// <summary>
        /// Creates an uninitialized, sealed room.
        /// </summary>
        /// <param name="name">The name of the room.</param>
        /// <param name="owner">The zone that owns this room.</param>
        /// <returns>Returns an uninitialized room instance</returns>
        Task<IRoom> CreateRoom(string name, IZone owner);
    }
}
