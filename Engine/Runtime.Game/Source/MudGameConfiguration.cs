using System;
using System.Collections.Generic;
using System.Text;

namespace MudDesigner.Runtime
{
    public class MudGameConfiguration : IGameConfiguration
    {
        private List<IAdapter> adapters = new List<IAdapter>();

        public string Name { get; set; }

        public string Description { get; set; }

        public string Version { get; set; }
        public string WebSite { get; set; }
    }
}
