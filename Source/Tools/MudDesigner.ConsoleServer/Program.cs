//-----------------------------------------------------------------------
// <copyright file="MainClass.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.MudServer.Windows
{
    using System;
    using System.Threading.Tasks;
    using MudDesigner.Engine;
    using MudDesigner.Engine.Game;

    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("");
            Console.WriteLine("Starting Server");
            SetupMessageBrokering();
            
            var bootstrap = new Bootstrap();
            Task bootstrapTask = bootstrap.Initialize();
            Console.WriteLine("Server Initialized");
            //Wait for the engine to shut down.
            bootstrapTask.ContinueWith(task => Console.WriteLine("Server started.")).Wait();
        }

        static void SetupMessageBrokering()
        {
            MessageBrokerFactory.Instance.Subscribe<InfoMessage>(
                (msg, subscription) => Console.WriteLine(msg.Content));

            //MessageBrokerFactory.Instance.Subscribe<GameMessage>(
            //    (msg, subscription) => Console.WriteLine(msg.Content));

            //MessageBrokerFactory.Instance.Subscribe<PlayerCreatedMessage>(
            //    (msg, sub) => Console.WriteLine("Player connected."));

            //MessageBrokerFactory.Instance.Subscribe<PlayerDeletionMessage>(
            //    (msg, sub) =>
            //    {
            //        Console.WriteLine("Player disconnected.");
            //    });
        }
    }
}
