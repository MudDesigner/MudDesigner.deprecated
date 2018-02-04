using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MudEngine.Routing;

namespace MudEngine
{
    public class RouteConfiguration
    {
        public IRouter Router { get; }

        public RouteConfiguration(IRouter router)
        {
            this.Router = router;
        }

        public RouteConfiguration AddRoute<TRoute>() where TRoute : IRoute
        {
            this.Router.RegisterHandler<TRoute>();
            return this;
        }
    }

    public static class RouteHandlerExtensions
    {
        public static IGame AddRouting(this IGame game, Action<RouteConfiguration> config)
        {
            IRouter router = null;
            var configuration = new RouteConfiguration(router);
            config(configuration);
            game.UseAdapters(router);
            return game;
        }
    }

    public class Foo : IRoute
    {
        public string Name => throw new NotImplementedException();

        public string Description => throw new NotImplementedException();

        public Task<IRouteResult> Handle(IRouteContext context)
        {
            return Task.FromResult(context.AcceptRoute());
        }
    }
        public class Bar : IRoute
    {
        public string Name => throw new NotImplementedException();

        public string Description => throw new NotImplementedException();

        public Task<IRouteResult> Handle(IRouteContext context)
        {
            return Task.FromResult(context.FailRoute("Failed"));
        }
    }

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            IGame game = null;
            IRouter router = null;

            game.AddRouting(config => 
            {
                router = config.Router;
                config.AddRoute<Foo>().AddRoute<Bar>();
            });

            // Input received -> publish notification
            // Routing receives notification -> Turns notification into route context
            // Instantiates route handler -> gives handler context (configurable to be real-time or wait for update() calls)
            // handler processes command received based off context data
            // returns handler results -> results placed into context
            // Routing converts context into output -> publishes output
            // output receives notification -> presents to user

            // Integration points:
            // Routing receives notification -> API for letting middleware interact/react to notification data
            // multiple handlers can be registered for the same route -> order matters and can cancel further processing
            // routing aggregates results before placing into context -> API for letting middleware interact with context after aggregation
            // routing publishes context data -> middleware can subscribe and react
        }
    }

    public class DefaultRouter : IRouter
    {
        public string Name => throw new NotImplementedException();

        public string Description => throw new NotImplementedException();

        public IMessageBroker MessageBroker => throw new NotImplementedException();

        public Task Configure()
        {
            throw new NotImplementedException();
        }

        public Task Delete()
        {
            throw new NotImplementedException();
        }

        public async Task<IRouteContext> HandleRoutes(params IRoute[] routes)
        {
            IRouteContext context= null;

            // If there are multiple matching routes, loop through them.
            // evaluate if the route context after each iteration allows for the next route to handle it or not.
            var results = new List<IRouteResult>();
            foreach(IRoute route in routes)
            {
                IRouteResult currentResult = await route.Handle(context);
                results.Add(currentResult);
                if (currentResult.ResultType == ResultType.Done)
                {
                    continue;
                }

                // evaluate why this result failed or was accepted and handle it.
                break;
            }

            // aggregate the results into the context
            return context;
        }

        public Task Initialize()
        {
            this.MessageBroker.Subscribe<CommandMessage>(this.HandleIncommingCommand);

            return Task.CompletedTask;
        }

        public void RegisterHandler<THandler>() where THandler : IRoute
        {
            throw new NotImplementedException();
        }

        public Task Update()
        {
            throw new NotImplementedException();
        }

        private async Task HandleIncommingCommand(CommandMessage cmd, ISubscription subscription)
        {
            // take msg and arguments and put them into the context.
            // create route based off the message content (command)
            IRoute[] routes = this.GetRoutesForCommand(cmd);
            IRouteContext context = await this.HandleRoutes(routes);

            // pull the result details from the context and publish
            // var resultCommand = new ResultCommand(context.Results);
            // await this.MessageBroker.PublishAsync(resultCommand);
        }

        private IRoute[] GetRoutesForCommand(CommandMessage command)
        {
            return Array.Empty<IRoute>();
        }
    }
}
