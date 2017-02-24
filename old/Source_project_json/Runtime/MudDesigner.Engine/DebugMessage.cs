//-----------------------------------------------------------------------
// <copyright file="DebugMessage.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine
{
    /// <summary>
    /// Standard string based message
    /// </summary>
    [MessageVerbosity(MessageScope.High)]
    public sealed class DebugMessage : MessageBase<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DebugMessage"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public DebugMessage(string message)
        {
            this.Content = message;
        }
    }
}
