using System.Collections.Generic;
using System.Linq;

namespace MudDesigner.MudEngine.Commanding
{
    public class CommandingConfiguration : ICommandingConfiguration
    {
        private List<IAdapter> adapter;

        private IEnumerable<IActorCommand> actorCommands;

        public CommandingConfiguration(ICommandFactory commandFactory)
        {
            this.adapter = new List<IAdapter>();
            this.CommandFactory = commandFactory;
        }

        public ICommandFactory CommandFactory { get; }

        public IAdapter[] GetAdapters()
        {
            return this.adapter.ToArray();
        }

        public void UseAdapters(IEnumerable<IAdapter> adapters)
        {
            foreach(IAdapter adapter in adapters)
            {
                this.UseAdapter(adapter);
            }
        }

        public void UseAdapter<TAdapter>() where TAdapter : class, IAdapter, new()
        {
            this.adapter.Add(new TAdapter());
        }

        public void UseAdapter<TAdapter>(TAdapter component) where TAdapter : class, IAdapter
        {
            this.adapter.Add(component);
        }
    }
}
