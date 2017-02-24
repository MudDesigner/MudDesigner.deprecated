namespace MudDesigner.Runtime
{
    public sealed class DebugMessage : MessageBase<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DebugMessage"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public DebugMessage(string message)
        {
            this.Content = message;
        }
    }
}
