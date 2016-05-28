//-----------------------------------------------------------------------
// <copyright file="WorldLoadedArgs.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine.Game
{
    using System;

    /// <summary>
    /// Provides the world that was loaded
    /// </summary>
    public sealed class WorldLoadedArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorldLoadedArgs"/> class.
        /// </summary>
        /// <param name="world">The world.</param>
        public WorldLoadedArgs(IWorld world)
        {
            this.World = world;
        }

        /// <summary>
        /// Gets the world.
        /// </summary>
        public IWorld World { get; }
    }
}
