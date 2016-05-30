//-----------------------------------------------------------------------
// <copyright file="Bootstrap.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.MudServer
{
    using System;
    using System.Threading.Tasks;
    using MudEngine;
    using MudEngine.Commanding;
    using MudEngine.Networking;

    /// <summary>
    /// Bootstraps the startup process of the game and server
    /// </summary>
    public class Bootstrap
    {
        /// <summary>
        /// Gets the game.
        /// </summary>
        public IGame Game { get; private set; }

        /// <summary>
        /// Gets the server running the game.
        /// </summary>
        public IServer Server { get; private set; }

        /// <summary>
        /// Initializes the server and game.
        /// </summary>
        /// <param name="startedCallback">The callback to invoke when initalization is completed.</param>
        public Task Initialize()
        {
            // Server setup
            IServerConfiguration serverConfig = new ServerConfiguration();
            serverConfig.Port = 10000;
            var server = new StandardServer(new TestPlayerFactory(), new ConnectionFactory());
            server.Configure(serverConfig);
            server.Owner = "@Scionwest";
			this.Server = server;

            // Commanding setup
            var commandManager = new CommandManager();
            commandManager.Configure(new CommandingConfiguration(new CommandFactory(new Type[] { typeof(LoginCommand), typeof(WalkCommand) })));

            IGameConfiguration gameConfig = new GameConfiguration();
            gameConfig.UseAdapter(server);
            gameConfig.UseAdapter(commandManager);

            IGame game = new MudGame();
            game.Configure(gameConfig);
			this.Game = game;

            return game.StartAsync();
        }
    }
}
