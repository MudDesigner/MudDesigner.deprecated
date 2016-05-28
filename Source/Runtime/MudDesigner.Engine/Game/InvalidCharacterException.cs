
namespace MudDesigner.Engine.Game
{
    using System;

    /// <summary>
    /// An exception to be thrown when an ICharacter is in an invalid state.
    /// </summary>
    public class InvalidCharacterException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidCharacterException" /> class.
        /// </summary>
        /// <param name="character">The character.</param>
        /// <param name="message">The message.</param>
        public InvalidCharacterException(ICharacter character, string message) : base(message)
        {
            this.Character = character;
        }

        /// <summary>
        /// Gets the character that is in an invalid state.
        /// </summary>
        public ICharacter Character { get; }
    }
}
