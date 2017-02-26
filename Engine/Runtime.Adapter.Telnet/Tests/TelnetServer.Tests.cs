using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MudDesigner.Runtime.Networking;
using System;
using System.Threading.Tasks;

namespace MudDesigner.Runtime.Adapter.Telnet
{
    [TestClass]
    public class TelnetServerTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task ExceptionThrownWhenInitalizingWithoutConfiguring()
        {
            // Arrange
            IServer server = TelnetServerTestContext.CreateTestContext().Server;

            // Act
            await server.Initialize();

            // Assert
            Assert.Fail("Expected exception was not thrown.");
        }

        [TestMethod]
        public async Task InitializingServerWillInitializeServerContext()
        {
            // Arrange
            // Server Context Mockup
            TelnetServerTestContext testContext = TelnetServerTestContext.CreateTestContext();
            IServer server = testContext.Server;
            await server.Configure();

            // Act
            await server.Initialize();

            // Assert
            testContext.ServerContextMock.Verify(mock => mock.Initialize(), Times.Exactly(1), "Server context was not initialized with the server.");
        }

        [TestMethod]
        public async Task DeletingServerWillDeleteServerContext()
        {
            // Arrange
            // Server Context Mockup
            TelnetServerTestContext testContext = TelnetServerTestContext.CreateTestContext();
            IServer server = testContext.Server;
            await server.Configure();

            // Act
            await server.Delete();

            // Assert
            testContext.ServerContextMock.Verify(mock => mock.Delete(), Times.Exactly(1), "Server context was not deleted with the server.");
        }
    }
}
