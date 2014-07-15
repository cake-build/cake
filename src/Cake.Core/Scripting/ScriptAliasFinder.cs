using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Cake.Core.Annotations;

namespace Cake.Core.Scripting
{
    public static class ScriptAliasFinder
    {
        public static IEnumerable<MethodInfo> GetExtensionMethods(IEnumerable<Assembly> references)
        {
            foreach (var reference in references)
            {
                foreach (var type in reference.GetTypes())
                {
                    if (type.IsStatic())
                    {
                        foreach (var extensionMethod in GetExtensionMethods(type))
                        {
                            yield return extensionMethod;
                        }
                    }
                }
            }
        }

        private static IEnumerable<MethodInfo> GetExtensionMethods(Type type)
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
