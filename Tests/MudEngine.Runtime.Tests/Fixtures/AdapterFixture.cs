using System;
using System.Threading.Tasks;

namespace MudDesigner.MudEngine.Tests.Fixtures
{
    public class AdapterFixture : AdapterBase
    {
        public override string Name => "Adapter fixture";

        public override void Configure()
        {
        }

        public override Task Delete() => Task.FromResult(0);

        public override Task Initialize() => Task.FromResult(0);

        public override Task Start(IGame game) => Task.FromResult(0);
    }
}
