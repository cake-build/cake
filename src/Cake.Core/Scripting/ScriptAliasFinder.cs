using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Cake.Core.Annotations;

namespace Cake.Core.Scripting
{
    /// <summary>
    /// Responsible for finding aliases in assemblies.
    /// </summary>
    public static class ScriptAliasFinder
    {
        /// <summary>
        /// Gets all alias extension methods in the list of assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        /// <returns>A list of methods.</returns>
        public static IEnumerable<MethodInfo> FindAliases(IEnumerable<Assembly> assemblies)
        {
            foreach (var reference in assemblies)
            {
                foreach (var type in reference.GetTypes())
                {
                    if (type.IsStatic())
                    {
                        foreach (var extensionMethod in GetAliasMethods(type))
                        {
                            yield return extensionMethod;
                        }
                    }
                }
            }
        }

        private static IEnumerable<MethodInfo> GetAliasMethods(Type type)
        {
            var methods = type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Static);
            foreach (var method in methods)
            {
                if (!method.IsDefined(typeof(ExtensionAttribute)))
                {
                    continue;
                }
                if (!method.IsDefined(typeof(CakeMethodAliasAttribute)) &&
                    !method.IsDefined(typeof(CakePropertyAliasAttribute)))
                {
                    continue;
                }

                var parameters = method.GetParameters();
                if (parameters.Length == 0)
                {
                    continue;
                }

                if (parameters[0].ParameterType == typeof(ICakeContext))
                {
                    yield return method;
                }
            }
        }
    }
}
