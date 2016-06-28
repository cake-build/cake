using Cake.Core.IO;
using System.Linq;
using System.Reflection;
using System;

#if !NET452
using Microsoft.Extensions.DependencyModel;
using System.Runtime.Loader;
#endif

namespace Cake.Core.Polyfill
{
    internal sealed class AssemblyLoader
    {
        public static Assembly LoadFromPath(FilePath path)
        {
            return LoadFromString(path.FullPath);
        }

        public static Assembly LoadFromString(string path)
        {
#if NET452
            return Assembly.LoadFrom(path);
#else
            throw new NotImplementedException("Not implemented for .NET Core.");
#endif
        }
    }
}
