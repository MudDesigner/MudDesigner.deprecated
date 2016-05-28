//-----------------------------------------------------------------------
// <copyright file="IDescriptor.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine
{
    /// <summary>
    /// Allows an object to be describable.
    /// </summary>
    public interface IDescriptor
    {
        /// <summary>
        /// Gets the name of the object.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets or sets a description of the object.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Sets the name for the object.
        /// </summary>
        /// <param name="name">The name.</param>
        void SetName(string name);
    }
}
