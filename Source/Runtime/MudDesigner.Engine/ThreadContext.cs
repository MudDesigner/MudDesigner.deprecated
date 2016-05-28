//-----------------------------------------------------------------------
// <copyright file="ThreadContext.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine
{
    using System;
    using System.Threading;

    /// <summary>
    /// Provides methods for invoking a callback onto a specific SynchronizationContext
    /// </summary>
    public class ThreadContext
    {
        /// <summary>
        /// The context to invoke the callback on to
        /// </summary>
        readonly SynchronizationContext context;

        /// <summary>
        /// The callback to invoke
        /// </summary>
        readonly Action callback;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadContext"/> class.
        /// </summary>
        /// <param name="syncContext">The synchronize context to invoke the callback on to.</param>
        /// <param name="callback">The callback to invoke.</param>
        public ThreadContext(SynchronizationContext syncContext, Action callback)
        {
            this.callback = callback;
            this.context = syncContext;
        }

        /// <summary>
        /// Invokes a callback onto the SynchronizationContext provided.
        /// </summary>
        public void Invoke()
        {
            this.context?.Post(this.ContextHandler, null);
        }

        /// <summary>
        /// Handles the SynchronizationContext posting.
        /// </summary>
        /// <param name="item">Parameter required by the synchronization context posting. It should always be null.</param>
        void ContextHandler(object item)
        {
            this.callback?.Invoke();
        }
    }

    /// <summary>
    /// Provides methods for invoking a callback onto a specific SynchronizationContext
    /// </summary>
    /// <typeparam name="T">The parameter being passed into the callback</typeparam>
    public sealed class ThreadContext<T>
    {
        /// <summary>
        /// The context to invoke the callback on to
        /// </summary>
        readonly SynchronizationContext context;

        /// <summary>
        /// The callback to invoke
        /// </summary>
        readonly Action<T> callback;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadContext{T}"/> class.
        /// </summary>
        /// <param name="syncContext">The synchronize context to invoke the callback on to.</param>
        /// <param name="callback">The callback to invoke.</param>
        public ThreadContext(SynchronizationContext syncContext, Action<T> callback)
        {
            this.callback = callback;
            this.context = syncContext;
        }

        /// <summary>
        /// Invokes a callback onto the SynchronizationContext provided.
        /// </summary>
        /// <param name="item">The item passed into the callback.</param>
        public void Invoke(T item)
        {
            this.context?.Post(this.ContextHandler, item);
        }

        /// <summary>
        /// Handles the SynchronizationContext posting.
        /// </summary>
        /// <param name="item">The parameter to pass into the callback.</param>
        private void ContextHandler(object item)
        {
            this.callback?.Invoke((T)item);
        }
    }
}
