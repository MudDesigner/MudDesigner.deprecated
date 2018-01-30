using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace MudEngine.Core
{
    public class ObjectPool<TObject> where TObject : class, IReusable, new()
    {
        private int maxPoolSize;
        private Stack<TObject> poolCache;

        private Action<TObject> objectReset;
        private Func<TObject> factory;

        public ObjectPool(IEnumerable<TObject> pool)
        {
            this.poolCache = new Stack<TObject>(pool);
            this.maxPoolSize = this.poolCache.Count;
        }

        public ObjectPool(int poolSize)
        {
            this.maxPoolSize = poolSize;
            this.poolCache = new Stack<TObject>();
        }

        public ObjectPool(int poolSize, Action<TObject> objectReset) : this(poolSize)
            => this.objectReset = objectReset;

        public ObjectPool(int poolSize, Func<TObject> factory) : this(poolSize)
            => this.factory = factory;

        public ObjectPool(int poolSize, Func<TObject> factory, Action<TObject> objectReset) : this(poolSize, objectReset)
            => this.factory = factory;

        public TObject Rent()
        {
            Monitor.Enter(this.poolCache);
            if (this.poolCache.Count == 0)
            {
                Monitor.Exit(this.poolCache);

                // New instances don't need to be prepared for re-use, so we just return it.
                return this.CreateNewInstance();
            }

            TObject instance = this.poolCache.Pop();
            Monitor.Exit(this.poolCache);
            this.TryResettingInstance(instance);

            return instance;
        }

        public void Return(TObject instanceObject)
        {
            Monitor.Enter(poolCache);
            if (this.poolCache.Count >= this.maxPoolSize)
            {
                Monitor.Exit(poolCache);
                return;
            }

            this.poolCache.Push(instanceObject);
            Monitor.Exit(poolCache);
        }

        private TObject CreateNewInstance() => this.factory == null
            ? new TObject()
            : this.factory();

        /// <summary>
        /// Resets the instance to a default state.
        /// </summary>
        /// <remarks>
        /// Instances are only reset when they are rented, and if they have been used before.
        /// They are never reset upon being returned to the pool in an effort to reduce overhead.
        /// If the instance is never used again then there is no need to reset it, so we only reset
        /// the instance upon it being used later in the future if needed.
        /// </remarks>
        /// <param name="instance">The instance that needs to be reset.</param>
        private void TryResettingInstance(TObject instance)
        {
            if (this.objectReset != null)
            {
                this.objectReset(instance);
                return;
            }

            instance.PrepareForReuse();
        }
    }
}
