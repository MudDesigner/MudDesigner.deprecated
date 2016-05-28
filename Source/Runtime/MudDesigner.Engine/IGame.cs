//-----------------------------------------------------------------------
// <copyright file="IGame.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Exposes properties and methods for managing the current game
    /// </summary>
    public interface IGame : IGameComponent
    {
        /// <summary>
        /// Gets a value indicating whether the game is currently running.
        /// </summary>
        /// <para>
        /// If false, it is possible that all of the objects are disabled or destroyed.
        /// </para>
        bool IsRunning { get; }

        /// <summary>
        /// Configures the game using the provided game configuration.
        /// </summary>
        /// <param name="config">The configuration the game should use.</param>
        /// <returns>Returns an awaitable Task</returns>
        Task Configure(IGameConfiguration config);

        /// <summary>
        /// Starts game asynchronously. This will start a game loop that can be awaited. The loop will run until stopped.
        /// </summary>
        /// <returns>Returns an awaitable Task</returns>
        Task StartAsync();

        /// <summary>
        /// Stops the game from running.
        /// </summary>
        /// <returns>Returns an awaitable Task</returns>
        Task Stop();

        /// <summary>
        /// Starts the game using the begin/end async pattern. This method requires the caller to handle the process life-cycle management as a loop is not generated internally.
        /// </summary>
        /// <param name="startCompletedCallback">The delegate to invoke when the game startup has completed.</param>
        void BeginStart(Action<IGame> startCompletedCallback);
    }
}
