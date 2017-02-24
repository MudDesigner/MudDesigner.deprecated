using System;
using System.Collections.Generic;
using System.Linq;

namespace MudDesigner.MudEngine.Commanding
{
    public class CommandResult
    {
        public CommandResult(bool isCompleted)
        {
            this.IsCompleted = isCompleted;
            this.PlayerMessage = string.Empty;
        }

        public CommandResult (bool isCompleted, string playerMessage) : this(isCompleted)
        {
            if (string.IsNullOrEmpty(playerMessage))
            {
                throw new ArgumentNullException(nameof(playerMessage), "The player message must not be null or empty when used with this overload.");
            }

            this.PlayerMessage = playerMessage;
        }

        public bool IsCompleted { get; }

        public string PlayerMessage { get; }
    }
}