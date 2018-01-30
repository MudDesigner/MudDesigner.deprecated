namespace MudEngine
{
    public interface IDescriptor
    {
        /// <summary>
        /// The name of the item being described
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The description of the item being described.
        /// </summary>
        string Description { get; }
    }
}
