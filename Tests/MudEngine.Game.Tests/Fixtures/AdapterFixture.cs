using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MudDesigner.MudEngine;

namespace MudEngine.Game.Tests.Fixtures
{
    public class AdapterFixture : AdapterBase
    {
        public bool IsDeleted { get; private set; }

        public bool IsStarted { get; private set; }

        public bool IsInitialized { get; private set; }

        public override string Name => "Adapter Fixture";

        public override Task Delete()
        {
            this.IsDeleted = true;
            return Task.FromResult(0);
        }

        public override Task Initialize()
        {
            this.IsInitialized = true;
            return Task.FromResult(0);
        }

        public override Task Start(IGame game)
        {
            this.IsStarted = true;
            return Task.FromResult(0);
        }

        public override void Configure()
        {
        }
    }
}
