//-----------------------------------------------------------------------
// <copyright file="AttributeCache.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Provides methods for fetching attributes from a cache instead of using reflection.
    /// </summary>
    public static class AttributeCache
    {
        /// <summary>
        /// Gets all attributes for a given Type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="property">The property.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Returns a collection of attributes</returns>
        public static IEnumerable<Attribute> GetAttributes(Type type, PropertyInfo property = null, Func<Attribute, bool> predicate = null)
        {
            CachedTypeData cacheType = ReflectedCache.TypePropertyCache.GetOrAdd(type, new CachedTypeData(type));
            return cacheType.GetAttributes(property, predicate);
        }

        /// <summary>
        /// Gets all attributes for a given Type.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="property">The property.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        /// Returns a collection of attributes
        /// </returns>
        public static IEnumerable<TAttribute> GetAttributes<TAttribute, TType>(PropertyInfo property = null, Func<Attribute, bool> predicate = null)
            where TAttribute : Attribute
            where TType : class
        {
            CachedTypeData cacheType = ReflectedCache.TypePropertyCache.GetOrAdd(typeof(TType), new CachedTypeData(typeof(TType)));
            return cacheType.GetAttributes<TAttribute>(property, predicate);
        }

        /// <summary>
        /// Gets all attributes for a given Type.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="type">The type.</param>
        /// <param name="property">The property.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Returns a collection of Attributes</returns>
        public static IEnumerable<TAttribute> GetAttributes<TAttribute>(Type type, PropertyInfo property = null, Func<Attribute, bool> predicate = null)
            where TAttribute : Attribute
        {
            CachedTypeData cacheType = ReflectedCache.TypePropertyCache.GetOrAdd(type, new CachedTypeData(type));
            return cacheType.GetAttributes<TAttribute>(property, predicate);
        }

        /// <summary>
        /// Gets the type of the attributes for.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="type">The type.</param>
        /// <param name="property">The property.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Returns a collection of Attributes</returns>
        public static IEnumerable<TAttribute> GetAttributes<TAttribute, TType>(TType type, PropertyInfo property = null, Func<TAttribute, bool> predicate = null)
            where TAttribute : Attribute
            where TType : class
        {
            Type desiredType = type == null ? typeof(TType) : type.GetType();
            CachedTypeData cacheType = ReflectedCache.TypePropertyCache.GetOrAdd(desiredType, new CachedTypeData(desiredType));
            return cacheType.GetAttributes<TAttribute>(property, predicate);
        }

        /// <summary>
        /// Gets the single attribute for Type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="property">The property.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Returns an Attribute</returns>
        public static Attribute GetAttribute(Type type, PropertyInfo property = null, Func<Attribute, bool> predicate = null)
        {
            CachedTypeData cacheType = ReflectedCache.TypePropertyCache.GetOrAdd(type, new CachedTypeData(type));
            return cacheType.GetAttribute(property, predicate);
        }

        /// <summary>
        /// Gets the single attribute matching TAttribute.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="type">The type.</param>
        /// <param name="property">The property.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Returns an Attribute</returns>
        public static TAttribute GetAttribute<TAttribute>(Type type, PropertyInfo property = null, Func<TAttribute, bool> predicate = null) where TAttribute : Attribute
        {
            CachedTypeData cacheType = ReflectedCache.TypePropertyCache.GetOrAdd(type, new CachedTypeData(type));
            return cacheType.GetAttribute<TAttribute>(property, predicate);
        }

        /// <summary>
        /// Gets the type of the attribute for.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="property">The property.</param>
        /// <param name="type">The type.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Returns an Attribute</returns>
        public static TAttribute GetAttribute<TAttribute, TType>(PropertyInfo property = null, TType type = null, Func<TAttribute, bool> predicate = null)
            where TAttribute : Attribute
            where TType : class
        {
            Type desiredType = type == null ? typeof(TType) : type.GetType();
            CachedTypeData cacheType = ReflectedCache.TypePropertyCache.GetOrAdd(desiredType, new CachedTypeData(desiredType));
            return cacheType.GetAttribute<TAttribute>(property, predicate);
        }
    }
}
