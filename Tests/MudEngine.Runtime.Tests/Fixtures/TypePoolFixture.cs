using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudDesigner.MudEngine.Tests.Fixture
{
    public class TypePoolFixture : ComponentFixture
    {
        [AttributeFixture]
        [AttributeFixture(true)]
        [System.ComponentModel.DefaultValue(5)]
        public int PropertyWithAttribute { get; set; } = 15;

        public double DoubleNumber { get; set; }

        public string StringFixture { get; set; }

        public long LongFixture { get; set; }

        public bool BoolFixture { get; set; }

        public double Totals { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;
    }
}
