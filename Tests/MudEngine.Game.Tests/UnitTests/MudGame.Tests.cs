using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MudDesigner.MudEngine;
using MudEngine.Game.Tests.Fixtures;

namespace MudEngine.Game.Tests.UnitTests
{
    [TestClass]
    public class MudGameTests
    {
        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Game")]
        [Owner("Johnathon Sullinger")]
        public async Task Configuring_a_game_assigns_the_game_configuration_instance()
        {
            // Arrange
            var configuration = Mock.Of<IGameConfiguration>();
            var game = new MudGame();

            // Act
            await game.Configure(configuration);

            // Assert
            Assert.IsNotNull(game.Configuration);
            Assert.AreEqual(configuration, game.Configuration);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Game")]
        [Owner("Johnathon Sullinger")]
        public async Task BeginStart_invokes_callback()
        {
            // Arrange
            var configuration = Mock.Of<IGameConfiguration>();
            var game = new MudGame();
            await game.Configure(configuration);
            var taskCompletionSource = new TaskCompletionSource<bool>();

            // Act
            game.BeginStart(g =>
            {
                taskCompletionSource.SetResult(true);
            });

            bool result = await taskCompletionSource.Task;

            // Assert
            Assert.IsTrue(result, "Callback was not hit.");
            Assert.IsTrue(game.IsRunning);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Game")]
        [Owner("Johnathon Sullinger")]
        public async Task BeginStart_with_null_synchronizationcontext_does_not_throw_exception()
        {
            // Arrange
            var configuration = Mock.Of<IGameConfiguration>();
            var game = new MudGame();
            await game.Configure(configuration);

            // We must ensure the synchronization context is null to test that the MudGame creates one itself
            SynchronizationContext.SetSynchronizationContext(null);

            // Act
            var taskCompletionSource = new TaskCompletionSource<bool>();
            game.BeginStart(g => taskCompletionSource.TrySetResult(true));

            bool callbackHit = await taskCompletionSource.Task;

            // Assert
            Assert.IsTrue(callbackHit, "Callback was not hit.");
            Assert.IsTrue(game.IsRunning);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Game")]
        [Owner("Johnathon Sullinger")]
        public async Task Start_runs_the_game()
        {
            // Arrange
            var configuration = Mock.Of<IGameConfiguration>();
            var game = new MudGame();
            await game.Configure(configuration);

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                              // Act
            Task.Run(async () => await game.StartAsync());
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            await Task.Delay(TimeSpan.FromSeconds(2));

            // Assert
            Assert.IsTrue(game.IsRunning);
            Assert.IsTrue(game.IsEnabled);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Game")]
        [Owner("Johnathon Sullinger")]
        public async Task Initialize_enables_the_game()
        {
            // Arrange
            var configuration = Mock.Of<IGameConfiguration>();
            var game = new MudGame();
            await game.Configure(configuration);

            // Act
            await game.Initialize();

            // Assert
            Assert.IsTrue(game.IsEnabled);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Game")]
        [Owner("Johnathon Sullinger")]
        public async Task Start_game_will_start_adapter()
        {
            // Arrange
            var configuration = Mock.Of<IGameConfiguration>(mock => mock.GetAdapters() == new IAdapter[1] 
            {
                new AdapterFixture()
            });
            var game = new MudGame();
            await game.Configure(configuration);

            IAdapter[] adapter = game.Configuration.GetAdapters();

            // Act
            game.BeginStart(runningGame => { });
            
            while (!game.IsRunning)
            {
                await Task.Delay(1);
            }

            // Assert
            Assert.IsTrue(((AdapterFixture)adapter[0]).IsInitialized);
            Assert.IsTrue(((AdapterFixture)adapter[0]).IsStarted);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Game")]
        [Owner("Johnathon Sullinger")]
        public async Task Stop_game_will_delete_adapter()
        {
            // Arrange
            var configuration = Mock.Of<IGameConfiguration>(mock => mock.GetAdapters() == new IAdapter[1] { new AdapterFixture() });
            var game = new MudGame();
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            await game.Configure(configuration);

            IAdapter[] adapters = game.Configuration.GetAdapters();

            // Act
            game.BeginStart(async (runningGame) => await game.Stop());
            var timer = new EngineTimer<AdapterFixture>((AdapterFixture)adapters[0]);
            timer.Start(TimeSpan.FromSeconds(20).TotalMilliseconds, 0, 1, (fixture, runningTimer) =>
            {
                runningTimer.Stop();
            });

            while(!((AdapterFixture)adapters[0]).IsDeleted)
            {
                await Task.Delay(1);
                if (!timer.IsRunning)
                {
                    break;
                }
            }

            // Assert
            Assert.IsTrue(((AdapterFixture)adapters[0]).IsDeleted);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Game")]
        [Owner("Johnathon Sullinger")]
        public async Task Delete_game_will_delete_adapter()
        {
            // Arrange
            var configuration = Mock.Of<IGameConfiguration>(mock => mock.GetAdapters() == new IAdapter[1] { new AdapterFixture() });
            var game = new MudGame();
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            await game.Configure(configuration);

            IAdapter[] adapters = game.Configuration.GetAdapters();

            // Act
            game.BeginStart(async (runningGame) => await game.Delete());
            var timer = new EngineTimer<AdapterFixture>((AdapterFixture)adapters[0]);
            timer.Start(TimeSpan.FromSeconds(20).TotalMilliseconds, 0, 1, (fixture, runningTimer) =>
            {
                runningTimer.Stop();
            });

            while (!((AdapterFixture)adapters[0]).IsDeleted)
            {
                await Task.Delay(1);
                if (!timer.IsRunning)
                {
                    break;
                }
            }

            // Assert
            Assert.IsTrue(((AdapterFixture)adapters[0]).IsDeleted);
        }
    }
}
