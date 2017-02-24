using System;
using System.Collections.Generic;
using System.Text;

namespace MudDesigner.Runtime
{
    public interface ISecurityPolicy
    {
        bool Verify();
    }
}
