//-----------------------------------------------------------------------
// <copyright file="IRealmFactory.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine.Game
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides methods for creating an instance of an IRealm implementation
    /// </summary>
    public interface IRealmFactory
    {
        /// <summary>
        /// Creates an unintialized instance of a realm.
        /// </summary>
        /// <param name="name">The name of the realm.</param>
        /// <param name="owner">The world that owns this realm.</param>
        /// <returns>Returns an unintialized instance of IRealm</returns>
        Task<IRealm> CreateRealm(string name, IWorld owner);

        /// <summary>
        /// Creates an unintialized instance of a realm.
        /// </summary>
        /// <param name="name">The name of the realm.</param>
        /// <param name="owner">The world that owns this realm.</param>
        /// <param name="timeZoneOffset">The time zone offset to apply to the realm.</param>
        /// <returns>Returns an unintialized instance of IRealm</returns>
        Task<IRealm> CreateRealm(string name, IWorld owner, ITimeOfDay timeZoneOffset);

        /// <summary>
        /// Creates an unintialized instance of a realm.
        /// All of the children zones will be initialized prior to being added to the realm.
        /// </summary>
        /// <param name="name">The name of the realm.</param>
        /// <param name="owner">The world that owns this realm.</param>
        /// <param name="zones">A collection of zones that will be initialized and added to the realm.</param>
        /// <returns>Returns an unintialized instance of IRealm</returns>
        Task<IRealm> CreateRealm(string name, IWorld owner, IEnumerable<IZone> zones);

        /// <summary>
        /// Creates an unintialized instance of a realm.
        /// All of the children zones will be initialized prior to being added to the realm.
        /// </summary>
        /// <param name="name">The name of the realm.</param>
        /// <param name="owner">The world that owns this realm.</param>
        /// <param name="timeZoneOffset">The time zone offset to apply to the realm.</param>
        /// <param name="zones">A collection of zones that will be initialized and added to the realm.</param>
        /// <returns>Returns an unintialized instance of IRealm</returns>
        Task<IRealm> CreateRealm(string name, IWorld owner, ITimeOfDay timeZoneOffset, IEnumerable<IZone> zones);
    }
}
