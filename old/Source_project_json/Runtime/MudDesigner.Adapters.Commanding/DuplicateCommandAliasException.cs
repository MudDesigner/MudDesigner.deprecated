using System;
using MudDesigner.Engine.Game;

namespace  MudDesigner.Adapters.Commanding
{
    public class DuplicateCommandAliasException : Exception
    {
        public DuplicateCommandAliasException(IActorCommand command)
        {
            this.DuplicateCommand = command;
        }
        
        public IActorCommand DuplicateCommand { get; }
    }
}