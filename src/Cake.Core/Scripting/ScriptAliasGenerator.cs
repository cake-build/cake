using System;
using System.Collections.Generic;
using System.Reflection;
using Cake.Core.Annotations;
using Cake.Core.Diagnostics;
using Cake.Core.Scripting.CodeGen;

namespace Cake.Core.Scripting
{
    /// <summary>
    /// The script alias generator.
    /// </summary>
    public sealed class ScriptAliasGenerator : IScriptAliasGenerator
    {
        private readonly ICakeLog _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptAliasGenerator"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        public ScriptAliasGenerator(ICakeLog log)
        {
            _log = log;
        }

        /// <summary>
        /// Generates script aliases and adds them to the specified session.
        /// </summary>
        /// <param name="context">The script context to add generated code to.</param>
        /// <param name="assemblies">The assemblies to find script aliases in.</param>
        public void GenerateScriptAliases(ScriptProcessorContext context, IEnumerable<Assembly> assemblies)
        {
            foreach (var method in ScriptAliasFinder.FindAliases(assemblies))
            {
                try
                {
                    // Import the method's namespace to the session.
                    var @namespace = method.GetNamespace();
                    context.AddNamespace(@namespace);

                    // Find out if the method want us to import more namespaces.
                    var namespaceAttributes = method.GetCustomAttributes<CakeNamespaceImportAttribute>();
                    foreach (var namespaceAttribute in namespaceAttributes)
                    {
                        context.AddNamespace(namespaceAttribute.Namespace);
                    }

                    // Find out if the class contains any more namespaces.
                    namespaceAttributes = method.DeclaringType.GetCustomAttributes<CakeNamespaceImportAttribute>();
                    foreach (var namespaceAttribute in namespaceAttributes)
                    {
                        context.AddNamespace(namespaceAttribute.Namespace);
                    }

                    // Generate the code.
                    var code = GenerateCode(method);

                    // Add the generated code to the context.
                    context.AddScriptAliasCode(code);
                }
                catch (Exception)
                {
                    // Log this error.
                    const string format = "An error occured while generating code for alias {0}.";
                    _log.Error(format, method.GetSignature(false));

                    // Rethrow the original exception.
                    throw;
                }
            }
        }

        private static string GenerateCode(MethodInfo method)
        {
            string code;
            if (method.IsDefined(typeof(CakeMethodAliasAttribute)))
            {
                code = MethodAliasGenerator.Generate(method);
            }
            else if (method.IsDefined(typeof(CakePropertyAliasAttribute)))
            {
                code = PropertyAliasGenerator.Generate(method);
            }
            else
            {
                throw new InvalidOperationException("Unknown alias type.");
            }
            return code;
        }
    }
}
