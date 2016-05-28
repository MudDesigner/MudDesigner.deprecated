//-----------------------------------------------------------------------
// <copyright file="CachedTypeData.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Represents a Type, its properties and attributes. This class holds cached reflection meta-data. 
    /// This lets objects ask for reflected meta-data without having to use reflection each time it needs to access the data.
    /// </summary>
    sealed class CachedTypeData
    {
        /// <summary>
        /// The attributes bag holds a cached collection of attributse for each property for the Type associated with an instance of CachedTypeData.
        /// </summary>
        readonly ConcurrentDictionary<PropertyInfo, IEnumerable<Attribute>> propertyAttributesBag;

        /// <summary>
        /// The properties bag holds a cached collection of properties for the Type associated with an instance of CachedTypeData.
        /// </summary>
        ConcurrentBag<PropertyInfo> propertiesBag;

        /// <summary>
        /// A thread-safe collection of every attribute associated with the Type associated with an instance of CachedTypeData.
        /// This collection includes all attributes for the Type including those that are stored in the propertyAttributesBag collection.
        /// </summary>
        ConcurrentBag<Attribute> typeAttributes;

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedTypeData"/> class for caching reflected meta-data for a given Type.
        /// </summary>
        /// <param name="type">The Type provided will have its reflected PropertyInfo and Attributes cached.</param>
        internal CachedTypeData(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type), "Type is required.");
            }

            this.Type = type;
            this.propertiesBag = new ConcurrentBag<PropertyInfo>();
            this.propertyAttributesBag = new ConcurrentDictionary<PropertyInfo, IEnumerable<Attribute>>();
            this.typeAttributes = new ConcurrentBag<Attribute>();
        }

        /// <summary>
        /// Gets the type that has its reflected PropertyInfo and Attributes cached.
        /// </summary>
        internal Type Type { get; }

        /// <summary>
        /// <para>
        /// Gets all of the attributes for the Type that is associated with an instance of CachedTypeData.
        /// If no property is specified then the method will return all attributes for the associated Type.
        /// </para>
        /// @code
        /// var cachedData = new CachedTypeData(typeof(DefaultGame));
        /// IEnumerable<Attribute> attributesForType = cachedData.GetAttributes();
        /// @endcode
        /// <para>
        /// You may optionally provided a predicate that will be used to filter out the results returned from the method.
        /// </para>
        /// @code
        /// var cachedData = new CachedTypeData(typeof(DefaultGame));
        /// IEnumerable<Attribute> attributesForType = cachedData.GetAttributes(null, attribute => attribute is CommandAliasAttribute);
        /// @endcode
        /// </summary>
        /// <param name="property">The optional property that you want to pull attributes from.</param>
        /// <param name="predicate">The optional predicate used to filter the attribute collection results.</param>
        /// <returns>Returns a collection of Attributes for the property specified.</returns>
        internal IEnumerable<Attribute> GetAttributes(PropertyInfo property = null, Func<Attribute, bool> predicate = null)
        {
            this.SetupAttributesBag();

            // If property is null, then return all attributes.
            var attributes = new List<Attribute>();
            if (property == null)
            {
                // build a collection of attributes
                foreach (var pair in this.propertyAttributesBag)
                {
                    attributes.AddRange(pair.Value);
                }

                attributes.AddRange(this.typeAttributes);

                // Return all attributes for the Type, filtered if needed
                return predicate == null
                    ? attributes
                    : attributes.Where(predicate);
            }

            // Fetch attributes for the given property
            attributes.AddRange(this.propertyAttributesBag.GetOrAdd(
                property,
                prop => prop.GetCustomAttributes(true).Cast<Attribute>()));

            // Return all attributes for property or filtered by predicate
            return predicate == null
                ? attributes
                : attributes.Where(predicate);
        }

        /// <summary>
        /// <param>
        /// Gets the first attribute available on the Type. You may provide an optional PropertyInfo to pull the Attribute off of. 
        /// In that case, the first Attribute on the given PropertyInfo will be returned.
        /// </param>
        /// @code
        /// var cachedData = new CachedTypeData(typeof(DefaultGame));
        /// var property = typeof(DefaultGame).GetProperties().First();
        /// Attribute fetchedAttribute = cachedData.GetAttribute(property);
        /// @endcode
        /// <param>
        /// An optional predicate can be provided in order to filter the attributes returned to you.
        /// </param>
        /// @code
        /// var cachedData = new CachedTypeData(typeof(DefaultGame));
        /// CommandAliasAttribute fetchedAttribute = (CommandAliasAttribute)cachedData.GetAttribute(attribute => attribute is CommandAliasAttribute);
        /// @endcode
        /// <para>
        /// If no attribute is found, null will be returned.
        /// </para>
        /// </summary>
        /// <param name="property">
        /// The optional property on the Type. 
        /// The attribute will be pulled off of this property. 
        /// If no property is specified, the attributes are pulled off of the Type.
        /// </param>
        /// <param name="predicate">The predicate used to filter the results returned from the method.</param>
        /// <returns>Returns the first Attribute found matching the parameters provided. If no parameters are given, the first Attribute on the Type is returned.</returns>
        internal Attribute GetAttribute(PropertyInfo property = null, Func<Attribute, bool> predicate = null) 
            => this.GetAttributes(property, predicate).FirstOrDefault();

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="property">The property.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Returns a collection of Attributes matching T</returns>
        internal IEnumerable<TAttribute> GetAttributes<TAttribute>(PropertyInfo property = null, Func<TAttribute, bool> predicate = null)
            where TAttribute : Attribute
        {
            this.SetupAttributesBag();

            if (property == null)
            {
                // If property is null, then return all attributes.
                var filteredAttributes = new List<TAttribute>();

                // build a collection of attributes
                foreach (var pair in this.propertyAttributesBag)
                {
                    filteredAttributes.AddRange(pair.Value.OfType<TAttribute>());
                }

                filteredAttributes.AddRange(this.typeAttributes.OfType<TAttribute>());

                // Return all attributes for the Type, filtered if needed
                return predicate == null
                    ? filteredAttributes
                    : filteredAttributes.Where(predicate);
            }

            // Fetch attributes for the given property
            var attributes = this.propertyAttributesBag.GetOrAdd(
                property,
                prop => prop.GetCustomAttributes(true).Cast<Attribute>());

            // Return all attributes for property or filtered by predicate
            return predicate == null
                ? attributes.OfType<TAttribute>()
                : attributes.OfType<TAttribute>().Where(predicate);
        }

        /// <summary>
        /// Gets the attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="property">The property.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Returns an Attribute matching T</returns>
        internal TAttribute GetAttribute<TAttribute>(PropertyInfo property = null, Func<TAttribute, bool> predicate = null) where TAttribute : Attribute
            => this.GetAttributes<TAttribute>(property, predicate).FirstOrDefault();

        /// <summary>
        /// Gets all of the properties.
        /// </summary>
        /// <returns>Returns a collection of PropertyInfo instances</returns>
        internal IEnumerable<PropertyInfo> GetProperties()
        {
            this.SetupPropertiesBag();
            return this.propertiesBag;
        }

        /// <summary>
        /// Gets the properties matching the predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Returns a collection of PropertyInfo instances</returns>
        internal IEnumerable<PropertyInfo> GetProperties(Func<PropertyInfo, bool> predicate)
        {
            this.SetupPropertiesBag();
            return this.propertiesBag.Where(predicate);
        }

        /// <summary>
        /// Gets a property matching the predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Returns a PropertyInfo</returns>
        internal PropertyInfo GetProperty(Func<PropertyInfo, bool> predicate)
        {
            this.SetupPropertiesBag();
            return this.propertiesBag.FirstOrDefault(predicate);
        }

        /// <summary>
        /// Setups the attributes bag.
        /// </summary>
        void SetupAttributesBag()
        {
            if (this.propertyAttributesBag.IsEmpty)
            {
                this.SetupPropertiesBag();
                foreach (PropertyInfo property in this.propertiesBag)
                {
                    this.propertyAttributesBag.AddOrUpdate(
                        property,
                        info => property.GetCustomAttributes(true).Cast<Attribute>(),
                        (propertyInfo, currentBag) => currentBag);
                }

                this.typeAttributes = new ConcurrentBag<Attribute>(
                    this.Type.GetTypeInfo().GetCustomAttributes(true).Cast<Attribute>());
            }
        }

        /// <summary>
        /// Setups the properties bag.
        /// </summary>
        void SetupPropertiesBag()
        {
            if (this.propertiesBag.IsEmpty)
            {
                this.propertiesBag = new ConcurrentBag<PropertyInfo>(this.Type.GetRuntimeProperties());
            }
        }
    }
}
