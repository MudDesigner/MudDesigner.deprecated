namespace MudDesigner.Runtime
{
    /// <summary>
    /// Allows for receiving the content of a message
    /// </summary>
    /// <typeparam name="TContent">The type of the content.</typeparam>
    public interface IMessage<TContent> : IMessage
    {
        /// <summary>
        /// Gets the content of the message.
        /// </summary>
        TContent Content { get; }
    }
}
