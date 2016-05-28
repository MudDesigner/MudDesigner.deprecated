//-----------------------------------------------------------------------
// <copyright file="TypeCache.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    /// <summary>
    /// Provides methods for fetching Types from a cache.
    /// </summary>
    public static class TypeCache
    {
        /// <summary>
        /// Gets the matching the provided predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Returns a collection of Types</returns>
        public static IEnumerable<Type> GetTypes(Func<Type, bool> predicate) => ReflectedCache.TypePropertyCache.Select(pair => pair.Key).Where(predicate);

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <typeparam name="T">The Type to fetch from cache</typeparam>
        /// <returns>Returns a cached Type</returns>
        public static Type GetType<T>() where T : class
        {
            CachedTypeData cacheType;
            Type itemType = typeof(T);
            ReflectedCache.TypePropertyCache.TryGetValue(itemType, out cacheType);

            if (cacheType != null)
            {
                return cacheType.Type;
            }

            cacheType = new CachedTypeData(itemType);
            ReflectedCache.TypePropertyCache.TryAdd(itemType, cacheType);

            return cacheType.Type;
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <typeparam name="T">The Type to fetch from cache</typeparam>
        /// <returns>Returns a cached Type</returns>
        public static Type GetType<T>(T item) where T : class
        {
            CachedTypeData cacheType;
            Type itemType = item.GetType();
            ReflectedCache.TypePropertyCache.TryGetValue(itemType, out cacheType);

            if (cacheType != null)
            {
                return cacheType.Type;
            }

            cacheType = new CachedTypeData(itemType);
            ReflectedCache.TypePropertyCache.TryAdd(itemType, cacheType);

            return cacheType.Type;
        }

        /// <summary>
        /// Adds the type.
        /// </summary>
        /// <param name="type">The type.</param>
        public static void AddType(Type type)
        {
            CachedTypeData cacheType;
            ReflectedCache.TypePropertyCache.TryGetValue(type, out cacheType);
            if (cacheType != null)
            {
                return;
            }

            cacheType = new CachedTypeData(type);
            ReflectedCache.TypePropertyCache.TryAdd(type, cacheType);
        }

        /// <summary>
        /// Gets the name of the type by.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Returns a cached Type</returns>
        public static Type GetTypeByName(string name)
        {
            var t = ReflectedCache.TypePropertyCache.Keys.FirstOrDefault(k => k.Name == name);

            return t;
        }

        /// <summary>
        /// Clears the type from pool.
        /// </summary>
        /// <typeparam name="T">The type to clear</typeparam>
        public static void ClearTypeFromPool<T>()
        {
            ClearTypeFromPool(typeof(T));
        }

        /// <summary>
        /// Clears the type from pool.
        /// </summary>
        /// <typeparam name="T">The Type to clear out of the cache</typeparam>
        /// <param name="item">The item.</param>
        public static void ClearTypeFromPool<T>(T item)
        {
            ClearTypeFromPool(item.GetType());
        }

        /// <summary>
        /// Clears the type from pool.
        /// </summary>
        /// <param name="typeToClear">The type to clear.</param>
        public static void ClearTypeFromPool(Type typeToClear)
        {
            CachedTypeData CachedTypeData;
            ReflectedCache.TypePropertyCache.TryRemove(typeToClear, out CachedTypeData);
        }

        /// <summary>
        /// Determines whether the given Type is already cached.
        /// </summary>
        /// <typeparam name="T">The Type you want to check if it has been cached.</typeparam>
        /// <returns>Returns true if the Type exists within the Pool</returns>
        public static bool HasTypeInCache<T>() => HasTypeInCache(typeof(T));

        /// <summary>
        /// Determines whether the given item has its Type in the pool.
        /// </summary>
        /// <typeparam name="T">The Type to perform the check against</typeparam>
        /// <param name="item">The item.</param>
        /// <returns>Returns True if the type exists in the cache</returns>
        public static bool HasTypeInCache<T>(T item) => HasTypeInCache(item.GetType());

        /// <summary>
        /// Determines whether the given Type is already cached.
        /// </summary>
        /// <param name="typeInCache">The Type you want to check if it has been cached.</param>
        /// <returns>Returns true if the Type exists within the Pool</returns>
        public static bool HasTypeInCache(Type typeInCache) => ReflectedCache.TypePropertyCache.ContainsKey(typeInCache);
    }
}
