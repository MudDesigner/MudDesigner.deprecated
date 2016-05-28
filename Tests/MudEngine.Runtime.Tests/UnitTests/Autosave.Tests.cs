using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MudDesigner.MudEngine.Tests.Fixture;

namespace MudDesigner.MudEngine.Tests
{
    [TestClass]
    public class AutosaveTests
    {
        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        [ExpectedException(typeof(ArgumentNullException))]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Potential Code Quality Issues", "RECS0026:Possible unassigned object created by 'new'", Justification = "<Pending>")]
        public void Constructor_throws_exception_with_null_item()
        {
            // Act
            new Autosave<ComponentFixture>(null, () => Task.FromResult(0));
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        [ExpectedException(typeof(ArgumentNullException))]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Potential Code Quality Issues", "RECS0026:Possible unassigned object created by 'new'", Justification = "<Pending>")]
        public void Constructor_throws_exception_with_null_delegate()
        {
            // Act
            new Autosave<ComponentFixture>(new ComponentFixture(), null);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        [ExpectedException(typeof(ArgumentNullException))]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Potential Code Quality Issues", "RECS0026:Possible unassigned object created by 'new'", Justification = "<Pending>")]
        public void Constructor_throws_exception_with_null_ctor_arguments()
        {
            // Act
            new Autosave<ComponentFixture>(null, null);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Is_not_running_when_created()
        {
            // Act
            var autosave = new Autosave<ComponentFixture>(new ComponentFixture(), () => Task.FromResult(0));

            // Assert
            Assert.IsFalse(autosave.IsAutosaveRunning, "Autosave started running before being initialized");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Is_running_when_initialized()
        {
            // Arrange
            var autosave = new Autosave<ComponentFixture>(new ComponentFixture(), () => Task.FromResult(0));

            // Act
            autosave.Initialize();

            // Assert
            Assert.IsTrue(autosave.IsAutosaveRunning, "Autosave did not start running once initialized");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public async Task Save_callback_invoked_when_running()
        {
            // Arrange
            bool callbackInvoked = false;
            var autosave = new Autosave<ComponentFixture>(
                new ComponentFixture(), 
                () =>
                {
                    callbackInvoked = true;
                    return Task.FromResult(0);
                });

            autosave.AutoSaveFrequency = 2;

            // Act
            await autosave.Initialize();
            await Task.Delay(TimeSpan.FromSeconds(3));
            
            // Assert
            Assert.IsTrue(callbackInvoked, "Autosave did not invoke the callback");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public async Task Delete_autosave_stops_running()
        {
            // Arrange
            var autosave = new Autosave<ComponentFixture>(new ComponentFixture(), () => Task.FromResult(0));
            await autosave.Initialize();

            // Act
            await autosave.Delete();

            // Assert
            Assert.IsFalse(autosave.IsAutosaveRunning, "Autosave was not stopped when deleted.");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public async Task Deleted_timer_can_reinitialize()
        {
            // Arrange
            bool callbackInvoked = false;
            var autosave = new Autosave<ComponentFixture>(
                new ComponentFixture(),
                () =>
                {
                    callbackInvoked = true;
                    return Task.FromResult(0);
                });
            autosave.AutoSaveFrequency = 2;
            await autosave.Initialize();

            // Act
            await autosave.Delete();
            await autosave.Initialize();
            await Task.Delay(TimeSpan.FromSeconds(3));

            // Assert
            Assert.IsTrue(callbackInvoked, "Autosave did not invoke the callback");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        [ExpectedException(typeof(ObjectDisposedException))]
        public async Task Initializing_a_disposed_autosave_throws_exception()
        {
            // Arrange
            var autosave = new Autosave<ComponentFixture>(new ComponentFixture(), () => Task.FromResult(0));
            await autosave.Initialize();

            // Act
            autosave.Dispose();
            await autosave.Initialize();
        }
    }
}
