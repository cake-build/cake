using System.Reflection;

namespace Cake.Core.Polyfill
{
    internal static class AssemblyHelper
    {
        public static Assembly GetExecutingAssembly()
        {
#if NET462
            return Assembly.GetExecutingAssembly();
#else
            return typeof(CakeEnvironment).GetTypeInfo().Assembly;
#endif
        }
    }
}
