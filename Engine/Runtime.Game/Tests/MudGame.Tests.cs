using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MudDesigner.Runtime.Game;
using System;
using System.Threading.Tasks;

namespace MudDesigner.Runtime.Tests
{
    [TestClass]
    public class MudGameTests
    {
        private MudGameConfiguration gameConfiguration;

        [TestInitialize]
        public void Setup()
        {
            string name = "foo bar";
            string description = "hello world, from description";
            this.gameConfiguration = new MudGameConfiguration { Name = name, Description = description };
        }

        [TestMethod]
        public void CtorSetsConfigurationReference()
        {
            // Arrange
            IGame game = null;
            IMessageBrokerFactory brokerFactory = Mock.Of<IMessageBrokerFactory>();

            // Act
            game = new MudGame(this.gameConfiguration, Mock.Of<IUniverseClock>(), brokerFactory);

            // Assert
            Assert.IsTrue(game.Configuration is MudGameConfiguration);

            MudGameConfiguration config = (MudGameConfiguration)game.Configuration;
            Assert.AreEqual(config.Name, this.gameConfiguration.Name);
            Assert.AreEqual(config.Description, this.gameConfiguration.Description);
        }

        [TestMethod]
        public async Task ConfigureInvokesAdapterConfiguration()
        {
            // Arrange
            IMessageBrokerFactory brokerFactory = Mock.Of<IMessageBrokerFactory>(brokerMock => brokerMock.CreateBroker() == Mock.Of<IMessageBroker>());
            IGame game = new MudGame(this.gameConfiguration, Mock.Of<IUniverseClock>(), brokerFactory);
            var mock = new Mock<IAdapter>();
            game.UseAdapter(mock.Object);

            // Act
            await game.Configure();

            // Assert
            mock.Verify(mockAdapter => mockAdapter.Configure(), Times.Exactly(1), $"The Adapter did not have it's {nameof(IAdapter.Configure)} method invoked.");
        }

        [TestMethod]
        public async Task StartAsyncRunsSuppliedAdapters()
        {
            // Arrange
            IMessageBrokerFactory brokerFactory = Mock.Of<IMessageBrokerFactory>(brokerMock => brokerMock.CreateBroker() == Mock.Of<IMessageBroker>());
            IGame game = new MudGame(this.gameConfiguration, Mock.Of<IUniverseClock>(), brokerFactory);
            var mock = new Mock<IAdapter>();
            mock.Setup(mockAdapter => mockAdapter.Initialize())
                .Callback(async () => await game.StopAsync())
                .Returns(() => Task.CompletedTask);

            game.UseAdapter(mock.Object);

            // Act
            // We don't await so that the test doesn't hang in the event a bug is introduced and the
            // test never stops the game loop.
            await game.StartAsync();

            // Assert
            Assert.AreEqual(GameState.Stopped, game.State, "The game was never stopped.");
            mock.Verify(mockAdapter => mockAdapter.Initialize(), Times.Exactly(1), "The game never ran the adapter.");
        }

        [TestMethod]
        public async Task StartAsyncCancelsWhenStopIsCalled()
        {
            // Arrange
            IMessageBrokerFactory brokerFactory = Mock.Of<IMessageBrokerFactory>();
            IGame game = new MudGame(this.gameConfiguration, Mock.Of<IUniverseClock>(), brokerFactory);
            game.OnStateChanged += async (state) =>
            {
                if (state == GameState.Running)
                {
                    await game.StopAsync();
                }
            };

            // Act
            Task gameStartTask = game.StartAsync();

            // Safety check to ensure we can cancel the game loop in the event we regress and break
            // this tests ability to stop the game loop.
            Task gameStartCancellationTask = Task.Delay(1000).ContinueWith(task =>
            {
                if (game.State == GameState.Running)
                {
                    throw new InvalidOperationException("The game start was not aborted.");
                }
            });

            await Task.WhenAny(gameStartTask, gameStartCancellationTask);

            // Assert
            Assert.IsFalse(game.State == GameState.Running);
        }
    }
}
