using System.Reflection;

#if DOTNET5_4
using System.Runtime.Loader;
#endif

namespace Cake.Core.Utilities
{
    internal static class AssemblyLoader
    {
        public static Assembly Load(string path)
        {
#if DOTNET5_4
            var assemblyName = AssemblyLoadContext.GetAssemblyName(path);

            return AssemblyLoadContext.Default.LoadFromAssemblyName(assemblyName);
#else
            return Assembly.LoadFrom(path);
#endif
        }
    }
}