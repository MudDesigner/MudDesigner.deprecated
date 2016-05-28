using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudDesigner.MudEngine.Tests.Fixture
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class AttributeFixture : Attribute
    {
        public AttributeFixture()
        {

        }

        public AttributeFixture(bool enabled)
        {
            this.IsEnabled = enabled;
        }

        public bool IsEnabled { get; }
    }
}
