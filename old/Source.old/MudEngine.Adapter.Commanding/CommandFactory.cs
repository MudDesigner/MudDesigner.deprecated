using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MudDesigner.MudEngine.Commanding
{
    public class CommandFactory : ICommandFactory
    {
        private readonly IEnumerable<Type> availableTypes;

        public CommandFactory(IEnumerable<Type> commandTypes)
        {
            if (commandTypes == null)
            {
                throw new ArgumentNullException(nameof(commandTypes), $"You must provide a collection of {typeof(IActorCommand).Name}s.");
            }

            this.availableTypes = commandTypes.Where(
                type => type.GetTypeInfo().ImplementedInterfaces.Any(interfaceType => interfaceType == typeof(IActorCommand)));
        }

        public IActorCommand CreateCommand(string command)
        {
            Type commandType = this.FindCommandType(command);
            if (commandType == null)
            {
                throw new InvalidOperationException($"The {command} does not have a coresponding {typeof(IActorCommand).Name} assocaited with it.");
            }

            return (IActorCommand)Activator.CreateInstance(commandType);
        }

        public bool IsCommandAvailable(string command)
        {
            // TODO: Optimize to not use so many string allocations.
            return this.FindCommandType(command) != null;
        }

        private Type FindCommandType(string command)
        {
            return this.availableTypes.FirstOrDefault(type =>
            {
                string typeName = type.Name;
                if (typeName.EndsWith("command", StringComparison.OrdinalIgnoreCase))
                {
                    typeName = typeName.Remove(typeName.Length - "command".Length);
                }

                if (string.Equals(typeName, command, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                var commandName = type.GetTypeInfo().GetCustomAttribute<CommandNameAttribute>();
                if (commandName == null)
                {
                    return false;
                }

                return string.Equals(typeName, commandName.Name, StringComparison.OrdinalIgnoreCase);
            });
        }
    }
}
