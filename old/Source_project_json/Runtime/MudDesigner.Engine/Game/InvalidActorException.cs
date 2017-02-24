
namespace MudDesigner.Engine.Game
{
    using System;

    /// <summary>
    /// An exception that can be thrown when an actor is in an invalid state.
    /// </summary>
    public sealed class InvalidActorException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidActorException"/> class.
        /// </summary>
        /// <param name="actor">The actor.</param>
        /// <param name="message">The message.</param>
        public InvalidActorException(IActor actor, string message) : base(message)
        {
            this.Actor = actor;
        }

        /// <summary>
        /// Gets the actor that is invalid.
        /// </summary>
        public IActor Actor { get; }
    }
}
