//-----------------------------------------------------------------------
// <copyright file="IConfigurable.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine
{
    /// <summary>
    /// Allows a component to become configurable
    /// </summary>
    public interface IConfigurable
    {
        /// <summary>
        /// Configures this instance.
        /// </summary>
        void Configure();
    }

    /// <summary>
    /// Allows a component to become configurable using a strongly typed configuration class
    /// </summary>
    /// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
    public interface IConfigurable<TConfiguration> : IConfigurable where TConfiguration : IConfiguration
    {
        /// <summary>
        /// Configures the component using the given configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        void Configure(TConfiguration configuration);
    }
}
