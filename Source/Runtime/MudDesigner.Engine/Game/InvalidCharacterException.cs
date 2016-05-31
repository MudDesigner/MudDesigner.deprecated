
namespace MudDesigner.Engine.Game
{
    using System;

    /// <summary>
    /// An exception to be thrown when an ICharacter is in an invalid state.
    /// </summary>
    public class InvalidMobException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidMobException" /> class.
        /// </summary>
        /// <param name="mob">The character.</param>
        /// <param name="message">The message.</param>
        public InvalidMobException(IMob mob, string message) : base(message)
        {
            this.Mob = mob;
        }

        /// <summary>
        /// Gets the character that is in an invalid state.
        /// </summary>
        public IMob Mob { get; }
    }
}
