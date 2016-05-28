//-----------------------------------------------------------------------
// <copyright file="MessageScopeAttribute.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine
{
    using System;

    /// <summary>
    /// Provides a means to define what scope a message should be published at.
    /// This allows for filtering messages that are published if they do not meet a specific scope.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class MessageVerbosityAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageVerbosityAttribute"/> class.
        /// </summary>
        /// <param name="scoping">The scoping.</param>
        public MessageVerbosityAttribute(MessageScope scoping)
        {
            this.Scope = scoping;
        }

        /// <summary>
        /// Gets the scope that a message is intended for.
        /// </summary>
        public MessageScope Scope { get; }
    }

    /// <summary>
    /// Defines the message scopes that are supported.
    /// </summary>
    public enum MessageScope
    {
        Debug = 0,
        Low = 1,
        High = 2,
        Full = 3,
    }
}
