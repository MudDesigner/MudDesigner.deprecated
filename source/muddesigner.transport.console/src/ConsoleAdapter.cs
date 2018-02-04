using System;
using System.Threading.Tasks;
using MudEngine.Transport;

namespace MudEngine
{
    public class ConsoleAdapter : IAdapter
    {
        private ITransportPipeline pipeline;
        
        public ConsoleAdapter(IMessageBrokerFactory brokerFactory)
        {
            this.MessageBroker = brokerFactory.CreateBroker();
        }

        public string Name => "Console Transport Adapter";

        public string Description => "Transports input via the system console.";

        public IMessageBroker MessageBroker { get; }

        public Task Configure()
        {
            this.pipeline = new ConsolePipeline();
            return Task.CompletedTask;
        }

        public Task Delete()
        {
            throw new NotImplementedException();
        }

        public Task Initialize()
        {
            return Task.CompletedTask;
        }

        public async Task Update()
        {
            byte[] buffer = await this.pipeline.Input.Read();
            if (buffer.Length == 0)
            {
                return;
            }
            
            await this.MessageBroker.PublishAsync(new InputReceivedMessage(buffer));
        }
    }
}