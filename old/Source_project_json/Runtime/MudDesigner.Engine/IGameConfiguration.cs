//-----------------------------------------------------------------------
// <copyright file="IGameConfiguration.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides properties that are used to define a game
    /// </summary>
    public interface IGameConfiguration : IConfiguration
    {
        /// <summary>
        /// Gets or sets the description of the game.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets the name of the game.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        Version Version { get; set; }

        /// <summary>
        /// Gets or sets the website.
        /// </summary>
        string Website { get; set; }
        
        Task Configure(IGame game);
    }
}