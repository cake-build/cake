using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Cake.Core.Polyfill
{
    internal static class AssemblyHelper
    {
        public static Assembly GetExecutingAssembly()
        {
#if NET452
            return Assembly.GetExecutingAssembly();
#else
            return typeof(CakeEnvironment).GetTypeInfo().Assembly;
#endif
        }
    }
}
