//-----------------------------------------------------------------------
// <copyright file="InvalidAdapterStateException.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine
{
    using System;

    /// <summary>
    /// Thrown when an adapter is used while in an invalid state.
    /// </summary>
    public class InvalidAdapterStateException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidAdapterStateException"/> class.
        /// </summary>
        /// <param name="adapter">The invalid adapter.</param>
        /// <param name="message">The message explaining what caused the invalid state to happen.</param>
        public InvalidAdapterStateException(IAdapter adapter, string message) : base(message)
        {
            this.Adapter = adapter;
        }

        /// <summary>
        /// Gets the adapter in an invalid state.
        /// </summary>
        public IAdapter Adapter { get; }
    }
}
