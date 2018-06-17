using System;
using System.Text;
using System.Threading.Tasks;
using MudEngine.Transport;

namespace MudEngine
{
    public class ConsoleAdapter : IAdapter
    {
        private ITransportPipeline pipeline;
        private bool isRunning = false;
        private Task adapterProcess = default(Task);
        
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

        public async Task WriteMessage(string message) => await this.pipeline.Output.Flush(Encoding.UTF8.GetBytes(message));

        public Task Delete()
        {
            throw new NotImplementedException();
        }

        public Task Initialize()
        {
            // This Task runs forever in the background, as long as the game is running.
            this.pipeline = new ConsolePipeline();
            this.isRunning = true;
            this.adapterProcess = Task.Run(async () =>
            {
                while(this.isRunning)
                {
                    byte[] buffer = await this.pipeline.Input.Read();
                    if (buffer.Length == 0)
                    {
                        return;
                    }

                    await this.MessageBroker.PublishAsync(new InputReceivedMessage(buffer));
                }
            });

            return Task.CompletedTask;
        }

        public async Task Update()
        {
        }
    }
}