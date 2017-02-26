namespace MudDesigner.Runtime
{
    /// <summary>
    /// Provides methods for creating a message broker or defining an abstract factory for delegating the creation of a message broker
    /// </summary>
    public class SingletonMessageBrokerFactory : IMessageBrokerFactory
    {
        static IMessageBroker instance = new MessageBroker();

        /// <summary>
        /// Creates a new instance of a message broker.
        /// </summary>
        /// <returns>Returns an IMessageBroker implementation</returns>
        public IMessageBroker CreateBroker()
        {
            return instance;
        }
    }
}
