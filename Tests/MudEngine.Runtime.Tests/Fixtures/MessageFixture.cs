using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudDesigner.MudEngine.Tests.Fixtures
{
    public class MessageFixture<TFixtureContentType> : MessageBase<TFixtureContentType> where TFixtureContentType : class
    {
        public MessageFixture(TFixtureContentType content)
        {
            this.Content = content;
        }
    }
}
