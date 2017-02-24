//-----------------------------------------------------------------------
// <copyright file="ICloneableComponent.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine
{
    /// <summary>
    /// An implementation of the Prototype pattern requiring an object to support the cloning of itself.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICloneableComponent<T>
    {
        /// <summary>
        /// Clones the properties of this instance to a new instance.
        /// </summary>
        /// <para>
        /// Cloning does not guarantee that the internal state of an object will be cloned nor
        /// does it guarantee that the clone will be a deep clone or a shallow.
        /// </para>
        /// <returns>Returns a new instance with the properties of this instance copied to it.</returns>
        T Clone();
    }
}
