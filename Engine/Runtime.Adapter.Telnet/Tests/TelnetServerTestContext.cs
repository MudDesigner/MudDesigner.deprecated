//using Moq;
//using MudDesigner.Runtime.Networking;

//namespace MudDesigner.Runtime.Adapter.Telnet
//{
//    internal class TelnetServerTestContext
//    {
//        // Mocks
//        internal Mock<IServerContext> ServerContextMock { get; set; }
//        internal Mock<IServerContextFactory> ServerContextFactoryMock { get; set; }
//        internal Mock<IGame> GameMock { get; set; }

//        // Implementations
//        internal IServerConfiguration ServerConfiguration { get; set; }
//        internal IServer Server { get; set; }
//        internal IGame Game { get; set; }
//        internal IServerContextFactory ServerContextFactory { get; set; }
//        internal IServerContext ServerContext { get; set; }

//        internal static TelnetServerTestContext CreateTestContext(
//            Mock<IServerContext> serverContextMock = null,
//            Mock<IServerContextFactory> serverContextFactoryMock = null,
//            Mock<IGame> gameMock = null,
//            Mock<IServerConfiguration> serverConfigurationMock = null)
//        {
//            if (serverContextMock == null)
//            {
//                serverContextMock = new Mock<IServerContext>();
//            }

//            if (serverContextFactoryMock == null)
//            {
//                serverContextFactoryMock = new Mock<IServerContextFactory>();
//                serverContextFactoryMock.Setup(mock => mock.CreateForServer(It.IsAny<IServer>()))
//                    .Returns<IServer>((givenServer) => serverContextMock.Object);
//            }

//            if (connectionFactoryMock == null)
//            {
//                connectionFactoryMock = new Mock<IConnectionFactory<TelnetServer>>();
//            }

//            if (serverConfigurationMock == null)
//            {
//                serverConfigurationMock = new Mock<IServerConfiguration>();
//                serverConfigurationMock.SetupGet(mock => mock.ServerContextFactory)
//                    .Returns(() => serverContextFactoryMock.Object);
//            }

//            if (gameMock == null)
//            {
//                gameMock = new Mock<IGame>();
//            }

//            IServerContext serverContext = serverContextMock.Object;
//            IServerContextFactory serverContextFactory = serverContextFactoryMock.Object;
//            IConnectionFactory<TelnetServer> connectionFactory = connectionFactoryMock.Object;
//            IServerConfiguration serverConfig = serverConfigurationMock.Object;
//            IGame game = gameMock.Object;

//            IServer server = new TelnetServer(game, serverConfig, connectionFactory);
//            serverConfig.ServerContextFactory = serverContextFactory;

//            return new TelnetServerTestContext
//            {
//                ServerContextMock = serverContextMock,
//                ServerContextFactoryMock = serverContextFactoryMock,
//                ConnectionFactoryMock = connectionFactoryMock,
//                GameMock = gameMock,
//                ServerConfiguration = serverConfig,
//                Server = server,
//                ConnectionFactory = connectionFactory,
//                Game = game,
//                ServerContext = serverContext,
//                ServerContextFactory = serverContextFactory
//            };
//        }
//    }
//}
