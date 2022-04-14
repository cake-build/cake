using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cake.Core.Diagnostics;
using Cake.Core.Scripting;

namespace Cake.Infrastructure.Scripting
{
    public sealed class ReferenceAssemblyResolver : IReferenceAssemblyResolver
    {
        private static readonly Version VersionZero = new Version(0, 0, 0, 0);
        private readonly ICakeLog _log;

        public ReferenceAssemblyResolver(ICakeLog log)
        {
            _log = log;
        }

        public Assembly[] GetReferenceAssemblies()
        {
            IEnumerable<Assembly> TryGetReferenceAssemblies()
            {
                foreach (var reference in
#if NETCOREAPP3_1
            Basic.Reference.Assemblies.NetCoreApp31.All)
#elif NET5_0
            Basic.Reference.Assemblies.Net50.All)
#else
            Basic.Reference.Assemblies.Net60.All)
#endif
                {
                    Assembly assembly;
                    try
                    {
                        assembly = Assembly.Load(System.IO.Path.GetFileNameWithoutExtension(reference.FilePath));
                    }
                    catch (Exception ex)
                    {
                        _log.Debug(log => log("Failed to load {0}\r\nException: {1}", reference.FilePath, ex));
                        continue;
                    }

                    if (assembly == null)
                    {
                        continue;
                    }

                    yield return assembly;

                    foreach (var assemblyRefName in assembly.GetReferencedAssemblies())
                    {
                        if (assemblyRefName == null ||
                            assemblyRefName.Version == VersionZero)
                        {
                            continue;
                        }

                        Assembly assemblyRef;
                        try
                        {
                            assemblyRef = Assembly.Load(assemblyRefName);
                        }
                        catch (Exception ex)
                        {
                            _log.Debug(log => log("Failed to load {0}\r\nReference: {1}\r\n Exception: {2}", assemblyRefName, assembly, ex));
                            continue;
                        }

                        if (assemblyRef == null)
                        {
                            continue;
                        }

                        yield return assemblyRef;
                    }
                }
            }

            return TryGetReferenceAssemblies().ToArray();
        }
    }
}
