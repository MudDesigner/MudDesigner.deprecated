//-----------------------------------------------------------------------
// <copyright file="IAdapter.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine
{
    using System.Threading.Tasks;

    /// <summary>
    /// Provides an interface for creating adapters that the game can start and run
    /// </summary>
    public interface IAdapter : IConfigurable, IMessagingComponent, IInitializableComponent
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the configuration used to configure the adapter.
        /// </summary>
        IConfiguration Configuration { get; }

        /// <summary>
        /// Starts this adapter and allows it to run.
        /// </summary>
        /// <param name="game">The an instance of an initialized game.</param>
        /// <returns>
        /// Returns an awaitable Task
        /// </returns>
        Task Start(IGame game);
    }

    /// <summary>
    /// Provides a strongly typed configuration class for adapters to request
    /// </summary>
    /// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
    public interface IAdapter<TConfiguration> : IAdapter, IConfigurable<TConfiguration> where TConfiguration : IConfiguration
    {
        /// <summary>
        /// Gets the configuration used to configure the adapter.
        /// </summary>
        new TConfiguration Configuration { get; }
    }
}
