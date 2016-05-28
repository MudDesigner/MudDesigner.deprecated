//-----------------------------------------------------------------------
// <copyright file="CommandProcessedMessage.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.MudEngine
{
    /// <summary>
    /// Provides the command that has been processed by a component within the engine.
    /// </summary>
    public class CommandProcessedMessage : MessageBase<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandProcessedMessage"/> class.
        /// </summary>
        /// <param name="message">The message that was processed.</param>
        /// <param name="target">The target that the command was originally intended for.</param>
        public CommandProcessedMessage(string message, IComponent target)
        {
            this.Content = message;
            this.Target = target;
        }

        /// <summary>
        /// Gets the target of the command.
        /// </summary>
        public IComponent Target { get; }
    }
}
