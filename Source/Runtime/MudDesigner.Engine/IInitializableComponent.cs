//-----------------------------------------------------------------------
// <copyright file="IInitializableComponent.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine
{
    using System.Threading.Tasks;

    /// <summary>
    /// Provides methods for initializing objects used by the engine and cleaning up when they are no longer needed.
    /// </summary>
    public interface IInitializableComponent
    {
        /// <summary>
        /// Initializes the component.
        /// </summary>
        /// <returns>Returns an awaitable Task</returns>
        Task Initialize();

        /// <summary>
        /// Lets this instance know that it is about to go out of scope and disposed.
        /// The instance will perform clean-up of its resources in preperation for deletion.
        /// </summary>
        /// <para>
        /// Informs the component that it is no longer needed, allowing it to perform clean up.
        /// Objects registered to one of the two delete events will be notified of the delete request.
        /// </para>
        /// <returns>Returns an awaitable Task</returns>
        Task Delete();
    }
}
