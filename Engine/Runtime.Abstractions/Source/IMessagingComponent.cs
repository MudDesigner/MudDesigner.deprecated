namespace MudDesigner.Runtime
{
    public interface IMessagingComponent
    {
        /// <summary>
        /// Gets the message broker that will be used for publishing messages from this component.
        /// </summary>
        IMessageBroker MessageBroker { get; }
    }
}
