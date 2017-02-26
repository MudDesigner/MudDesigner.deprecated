using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MudDesigner.Runtime.Networking;
using Runtime.Adapter.Telnet.Tests.Fakes;

namespace MudDesigner.Runtime.Adapter.Telnet
{
    [TestClass]
    public class IGameServerExtensionsTests
    {
        [TestMethod]
        public void AddTelnetServerAddsAdapterToGame()
        {
            // Arrange
            var gameMock = new Mock<IGame>();
            IGame game = gameMock.Object;

            // Act
            game.AddTelnetServer();

            // Assert
            gameMock.Verify(mock => mock.UseAdapter(It.IsAny<IAdapter>()), Times.Exactly(1));
        }

        [TestMethod]
        public void AddTelnetServerConfiguresServer()
        {
            // Arrange
            var gameMock = Mock.Of<IGame>();
            var passwordPolicy = Mock.Of<ISecurityPolicy>();

            // Act
            IServer server = gameMock.AddTelnetServer(config => config.PasswordPolicy = passwordPolicy);

            // Assert
            Assert.AreEqual(passwordPolicy, server.Configuration.PasswordPolicy);
        }
    }
}
