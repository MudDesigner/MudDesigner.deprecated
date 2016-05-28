//-----------------------------------------------------------------------
// <copyright file="MessageBrokerFactory.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine
{
    /// <summary>
    /// Provides methods for creating a message broker or defining an abstract factory for delegating the creation of a message broker
    /// </summary>
    public class MessageBrokerFactory : IMessageBrokerFactory
    {
        static IMessageBroker instance;

        public static IMessageBroker Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MessageBroker();
                }

                return instance;
            }
        }
        
        public IMessageBroker GetInstance()
        {
            return MessageBrokerFactory.Instance;
        }

        /// <summary>
        /// Creates a new instance of a message broker.
        /// </summary>
        /// <returns>Returns an IMessageBroker implementation</returns>
        public IMessageBroker CreateBroker()
        {
            return new MessageBroker();
        }
    }
}
