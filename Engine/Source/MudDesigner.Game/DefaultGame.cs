using System;
using System.Threading.Tasks;

namespace MudEngine
{
    public class DefaultGame : IGame
    {
        private GameConfiguration configuration;
        private IAdapter[] adapters;
        private double lastUpdateTime;

        public DefaultGame(GameConfiguration gameConfiguration)
        {
            this.configuration = gameConfiguration;
        }

        public GameState CurrentState { get; private set; } = GameState.None;

        public Guid Id { get; private set; }

        public bool IsEnabled { get; private set; }

        public DateTime CreatedAt { get; } = DateTime.UtcNow;

        public double TimeAlive => DateTime.UtcNow.Subtract(this.CreatedAt).TotalMilliseconds;

        public IMessageBroker MessageBroker { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public async Task Configure()
        {
            await this.SetState(GameState.Configuring);

            this.MessageBroker = configuration.MessageBrokerFactory.CreateBroker();
            this.Name = this.configuration.GameName;
            this.Description = this.configuration.GameDescription;
            this.Id = this.configuration.GameId;
            
            await this.SetState(GameState.Configured);
        }

        public Task Delete()
        {
            throw new NotImplementedException();
        }

        public Task Disable()
        {
            this.IsEnabled = false;
            return Task.CompletedTask;
        }

        public Task Enable()
        {
            this.IsEnabled = false;
            return Task.CompletedTask;
        }

        public IAdapter[] GetAdapters() => this.adapters;

        public async Task Initialize()
        {
            await this.SetState(GameState.Starting);
            foreach(IAdapter adapter in this.adapters)
            {
                await adapter.Initialize();
            }

            // If an adapter aborted the startup, we delete.
            if (this.CurrentState != GameState.Starting)
            {
                await this.Delete();
                throw new ApplicationException("An adapter aborted the game initialization. The adapter that caused the cancellation is unknown. Evaluate any logs that the adapters might have produced. The game forced a self delete and attempted to clean up all used initialized resources.");
            }

            await this.SetState(GameState.Running);
            while(this.IsEnabled)
            {
                if (this.TimeAlive - this.lastUpdateTime < this.configuration.UpdateFrequency)
                {
                    continue;
                }

                foreach(IAdapter adapter in this.adapters)
                {
                    await adapter.Update();
                }
                
                this.lastUpdateTime = this.TimeAlive;
            }
        }

        public void UseAdapters(params IAdapter[] adaptersToUse)
        {
            foreach(IAdapter adapter in adaptersToUse)
            {
                if (adapter == null)
                {
                    throw new ArgumentNullException(nameof(adaptersToUse), "You can not provide a null adapter.");
                }
            }

            this.adapters = adaptersToUse;
        }

        private async Task SetState(GameState state)
        {
            this.CurrentState = state;
            await this.MessageBroker.PublishAsync(new GameStateChangedMessage(this));
        }
    }
}