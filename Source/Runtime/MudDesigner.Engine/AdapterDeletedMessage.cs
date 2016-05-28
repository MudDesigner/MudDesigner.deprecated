//-----------------------------------------------------------------------
// <copyright file="AdapterDeletedMessage.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine
{
    /// <summary>
    /// Allows for publishing the adapter being deleted through the message broker.
    /// </summary>
    public class AdapterDeletedMessage : MessageBase<IAdapter>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdapterDeletedMessage"/> class.
        /// </summary>
        /// <param name="adapter">The adapter being deleted.</param>
        public AdapterDeletedMessage(IAdapter adapter)
        {
            this.Content = adapter;
        }
    }
}
