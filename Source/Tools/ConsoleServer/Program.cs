//-----------------------------------------------------------------------
// <copyright file="MainClass.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.MudServer.Windows
{
    using MudDesigner.MudEngine.Actors;
    using MudDesigner.MudEngine.MessageBrokering;
    using System;
    using System.Threading;
    using MudEngine;
    using System.Threading.Tasks;
    
    class MainClass
    {
        public static void Main(string[] args)
        {
            SetupMessageBrokering();
            
            var bootstrap = new Bootstrap();
            Task bootstrapTask = bootstrap.Initialize();

            //Wait for the engine to shut down.
            bootstrapTask.Wait();
        }

        static void SetupMessageBrokering()
        {
            MessageBrokerFactory.Instance.Subscribe<InfoMessage>(
                (msg, subscription) => Console.WriteLine(msg.Content));

            MessageBrokerFactory.Instance.Subscribe<GameMessage>(
                (msg, subscription) => Console.WriteLine(msg.Content));

            MessageBrokerFactory.Instance.Subscribe<PlayerCreatedMessage>(
                (msg, sub) => Console.WriteLine("Player connected."));

            MessageBrokerFactory.Instance.Subscribe<PlayerDeletionMessage>(
                (msg, sub) =>
                {
                    Console.WriteLine("Player disconnected.");
                });
        }
    }
}
