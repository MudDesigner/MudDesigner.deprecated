using MudDesigner.Runtime.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MudDesigner.Runtime
{
    public sealed class MudGame : IGame
    {
        private List<IAdapter> adapters = new List<IAdapter>();

        public MudGame(MudGameConfiguration gameConfiguration, IUniverseClock universeClock, IMessageBrokerFactory brokerFactory)
        {
            this.Configuration = gameConfiguration;
            this.MessageBroker = brokerFactory.CreateBroker();
            this.UniverseClock = universeClock;
        }

        public event Func<GameState, Task> OnStateChanged;

        public IMessageBroker MessageBroker { get;  }

        public IUniverseClock UniverseClock { get; }

        public GameState State { get; private set; }

        public string Name { get; }

        public string Description { get; }

        public IGameConfiguration Configuration { get; }

        public async Task Configure()
        {
            this.SetState(GameState.Configuring);
            foreach (IAdapter adapter in this.adapters)
            {
                await adapter.Configure();
            }

            this.MessageBroker.Subscribe<CurrentUniverseTimeMessage>(
                (msg, sub) => { });

            this.SetState(GameState.Configured);
        }

        public IEnumerable<IAdapter> GetAdapters() => this.adapters;

        public void UseAdapter<TAdapter>(TAdapter adapter) where TAdapter : class, IAdapter
        {
            if (this.adapters.Any(existingAdapter => existingAdapter == adapter))
            {
                return;
            }

            this.adapters.Add(adapter);
        }

        public void UseAdapters(params IAdapter[] adaptersToUse)
        {
            foreach (IAdapter adapter in adaptersToUse)
            {
                this.UseAdapter(adapter);
            }
        }

        public async Task StartAsync()
        {
            this.SetState(GameState.Starting);
            foreach (IAdapter adapter in this.adapters)
            {
                await adapter.Initialize();
            }

            if (this.State != GameState.Starting)
            {
                await this.StopAsync();
                return;
            }

            this.SetState(GameState.Running);
            while (this.State == GameState.Running)
            {
                await this.UpdateComponents();
            }
        }

        public async Task StopAsync()
        {
            this.SetState(GameState.Stopping);
            foreach(IAdapter adapter in this.adapters)
            {
                await adapter.Delete();
            }

            this.SetState(GameState.Stopped);
        }

        private async Task UpdateComponents()
        {
            foreach (IAdapter component in this.adapters)
            {
                await component.Update(this);
            }
        }

        private void SetState(GameState state)
        {
            this.State = state;
            this.OnStateChanged?.Invoke(state);
            this.MessageBroker.Publish(new GameStateChangedMessage(this));
        }
    }
}
