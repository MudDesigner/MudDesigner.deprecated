using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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

            // Act
            game = new MudGame(this.gameConfiguration);

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
            IGame game = new MudGame(this.gameConfiguration);
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
            IGame game = new MudGame(this.gameConfiguration);
            var mock = new Mock<IAdapter>();
            mock.Setup(mockAdapter => mockAdapter.Run(It.IsAny<IGame>()))
                .Callback<IGame>(async (runningGame) => await game.StopAsync())
                .Returns(() => Task.CompletedTask);

            game.UseAdapter(mock.Object);

            // Act
            // We don't await so that the test doesn't hang in the event a bug is introduced and the
            // test never stops the game loop.
#pragma warning disable 4014
            // Disables the warning about not await this call.
            await game.StartAsync();
#pragma warning restore 4014

            // Assert
            Assert.AreEqual(GameState.Stopped, game.State, "The game was never stopped.");
            mock.Verify(mockAdapter => mockAdapter.Run(It.IsAny<IGame>()), Times.Exactly(1), "The game never ran the adapter.");
        }

        [TestMethod]
        public async Task StartAsyncCancelsWhenStopIsCalled()
        {
            // Arrange
            IGame game = new MudGame(this.gameConfiguration);
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
