using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cake.Core
{
    public interface ICakeArguments
    {
        bool HasArgument(string key);
        string GetArgument(string key);
    }
}
