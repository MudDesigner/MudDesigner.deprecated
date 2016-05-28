//-----------------------------------------------------------------------
// <copyright file="PropertyCache.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Provides methods for fetching PropertyInfo instances off an object without using reflection.
    /// </summary>
    public static class PropertyCache
    {
        /// <summary>
        /// Gets the properties for the provided Type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Returns a collection of PropertyInfo types</returns>
        public static IEnumerable<PropertyInfo> GetPropertiesForType(Type type, Func<PropertyInfo, bool> predicate = null)
        {
            CachedTypeData cacheType = ReflectedCache.TypePropertyCache.GetOrAdd(type, new CachedTypeData(type));

            return predicate == null
                ? cacheType.GetProperties()
                : cacheType.GetProperties(predicate);
        }

        /// <summary>
        /// Gets the properties associated with the given items Type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Returns a collection of PropertyInfo types</returns>
        public static IEnumerable<PropertyInfo> GetPropertiesForType<T>(Func<PropertyInfo, bool> predicate = null)
            where T : class
        {
            Type desiredType = typeof(T);
            return GetPropertiesForType(desiredType, predicate);
        }

        /// <summary>
        /// Gets the properties associated with the given items Type.
        /// </summary>
        /// <typeparam name="T">The type to get the property off of</typeparam>
        /// <param name="item">The item.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        /// Returns a collection of PropertyInfo types
        /// </returns>
        public static IEnumerable<PropertyInfo> GetPropertiesForType<T>(T item, Func<PropertyInfo, bool> predicate = null)
            where T : class
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "You must provide an instance of T");
            }

            Type desiredType = item.GetType();
            return GetPropertiesForType(desiredType, predicate);
        }

        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <typeparam name="T">The type to get the property off of</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="item">The item.</param>
        /// <returns>Returns a PropertyInfo</returns>
        public static PropertyInfo GetProperty<T>(Func<PropertyInfo, bool> predicate, T item = null)
            where T : class
        {
            Type desiredType = item == null ? typeof(T) : item.GetType();
            return GetProperty(desiredType, predicate);
        }

        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Returns a PropertyInfo</returns>
        public static PropertyInfo GetProperty(Type type, Func<PropertyInfo, bool> predicate)
        {
            CachedTypeData cacheType = ReflectedCache.TypePropertyCache.GetOrAdd(type, new CachedTypeData(type));
            return cacheType.GetProperty(predicate);
        }
    }
}
