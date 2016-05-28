using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudDesigner.MudEngine.Commanding
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class CommandNameAttribute : Attribute
    {
        public CommandNameAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; }
    }
}
