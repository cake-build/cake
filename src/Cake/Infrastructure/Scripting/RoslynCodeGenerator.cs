// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Core.Scripting;
using Cake.Core.Scripting.CodeGen;

namespace Cake.Infrastructure.Scripting
{
    public sealed class RoslynCodeGenerator
    {
        public string Generate(Script script)
        {
            var defines = string.Join("\r\n", script.Defines);
            var usingStaticDirectives = string.Join("\r\n", script.UsingStaticDirectives);
            var usingDirectives = string.Join("\r\n", script.UsingAliasDirectives);
            var aliases = GetAliasCode(script);
            var code = string.Join("\r\n", script.Lines);
            return string.Join("\r\n", defines, usingStaticDirectives, usingDirectives, aliases, code);
        }

        private static string GetAliasCode(Script context)
        {
            var result = new Dictionary<string, string>();
            foreach (var alias in context.Aliases)
            {
                string hash, code = alias.Type == ScriptAliasType.Method
                    ? MethodAliasGenerator.Generate(alias.Method, out hash)
                    : PropertyAliasGenerator.Generate(alias.Method, out hash);

                string @namespace = alias.Method.DeclaringType.Namespace ?? "@null";
                if (result.ContainsKey(hash))
                {
                    var message = $"{alias.Type.ToString().ToLowerInvariant()} \"{alias.Name}\" excluded from code generation and will need to be fully qualified on ICakeContext.";
                    if (context.ExcludedNamespaces.ContainsKey(@namespace))
                    {
                        context.ExcludedNamespaces[@namespace].Add(message);
                    }
                    else
                    {
                        context.ExcludedNamespaces.Add(@namespace,
                            new List<string>() { message });
                    }
                    continue;
                }
                else if (context.ExcludedNamespaces.ContainsKey(@namespace))
                {
                    var message = $"{alias.Type.ToString().ToLowerInvariant()} \"{alias.Name}\" was included in code generation, but will need to be fully qualified on ICakeContext.";
                    context.ExcludedNamespaces[@namespace].Add(message);
                }

                result.Add(hash, code);
            }
            return string.Join("\r\n", result.Values);
        }
    }
}