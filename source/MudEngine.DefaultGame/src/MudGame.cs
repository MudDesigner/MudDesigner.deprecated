using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MudEngine.Core;

namespace MudEngine.DefaultGame
{
    public class MudGame : IGame
    {
        private IAdapter[] adapters;
        private readonly GameConfiguration gameConfiguration;

        public MudGame(GameConfiguration gameConfiguration)
        {
            this.gameConfiguration = gameConfiguration;
        }

        public GameState CurrentState { get; }

        public Guid Id { get; } = Guid.NewGuid();

        public bool IsEnabled { get; private set; }

        public DateTime CreatedAt { get; } = DateTime.UtcNow;

        public double TimeAlive => (DateTime.UtcNow - this.CreatedAt).TotalMilliseconds;

        public IMessageBroker MessageBroker { get; }

        public string Name { get; internal set; }

        public string Description { get; internal set; }

        public async Task Configure()
        {
            this.Name = this.gameConfiguration.Name;
            this.Description = this.gameConfiguration.Description;
            
            foreach(IAdapter adapter in this.adapters)
            {
                await adapter.Configure();
            }
        }

        public Task Delete()
        {
            return Task.CompletedTask;
        }

        public Task Disable()
        {
            this.IsEnabled = false;
            return Task.CompletedTask;
        }

        public Task Enable()
        {
            this.IsEnabled = true;
            return Task.CompletedTask;
        }

        public IAdapter[] GetAdapters()
        {
            return this.adapters;
        }

        public void UseAdapters(params IAdapter[] adaptersToUse)
        {
            this.adapters = adaptersToUse;
        }

        public async Task Initialize()
        {
            foreach(IAdapter adapter in this.adapters)
            {
                await adapter.Initialize();
            }
        }
    }
}
