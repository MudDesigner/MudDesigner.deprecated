//-----------------------------------------------------------------------
// <copyright file="ServerContentParser.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Adapters.Server
{
    using System.Threading.Tasks;
    using MudDesigner.Engine;
    using MudDesigner.Engine.Game;

    /// <summary>
    /// The ServerContentParser sits as the mediator between commands and client socket messages.
    /// Command results and client socket messages are parsed and transformed into a format that
    /// each end of the tunnel needs in order to act upon the message content.
    /// </summary>
    internal class ServerContentParser : AdapterBase
    {
        private bool started;

        /// <summary>
        /// Gets the name.
        public override string Name => "Server Content Parser";

        /// <summary>
        /// Initializes the component.
        /// </summary>
        /// <returns>
        /// Returns an awaitable Task
        /// </returns>
        public override Task Initialize()
        {
            // this.SubscribeToMessage<CommandProcessedMessage>(
            //     (message, subscription) => { },
            //     (message) => started);

            this.SubscribeToMessage<ClientMessageReceived>(
                (message, subscription) => { },
                (message) => started && !string.IsNullOrEmpty(message.Content.Data) && message.Content.Player != null && message.Content.Connection != null);

            return Task.FromResult(0);
        }

        /// <summary>
        /// Starts this adapter and allows it to run.
        /// </summary>
        /// <param name="game">The an instance of an initialized game.</param>
        /// <returns>
        /// Returns an awaitable Task
        /// </returns>
        public override Task Start(IGame game)
        {
            this.started = true;
            return Task.FromResult(0);
        }

        /// <summary>
        /// Lets this instance know that it is about to go out of scope and disposed.
        /// The instance will perform clean-up of its resources in preperation for deletion.
        /// </summary>
        /// <returns>
        /// Returns an awaitable Task
        /// </returns>
        /// <para>
        /// Informs the component that it is no longer needed, allowing it to perform clean up.
        /// Objects registered to one of the two delete events will be notified of the delete request.
        /// </para>
        public override Task Delete()
        {
            this.UnsubscribeFromAllMessages();
            return Task.FromResult(0);
        }

        /// <summary>
        /// Configures this adapter for use within a game.
        /// </summary>
        public override void Configure()
        {
        }
    }
}
