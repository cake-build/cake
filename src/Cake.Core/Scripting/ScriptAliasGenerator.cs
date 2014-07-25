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
        /// <param name="session">The session to add script aliases to.</param>
        /// <param name="assemblies">The assemblies to find script aliases in.</param>
        public void Generate(IScriptSession session, IEnumerable<Assembly> assemblies)
        {
            _log.Debug("Generating script alias code...");

            foreach (var method in ScriptAliasFinder.FindAliases(assemblies))
            {
                try
                {
                    // Generate the code.
                    var code = GenerateCode(method);

                    // Import namespaces.
                    ImportNamespaces(session, method);

                    // Execute the code.
                    session.Execute(code);
                }
                catch (Exception)
                {
                    // Log this error.
                    const string format = "An error occured while generating proxy code for {0}.";
                    _log.Error(format, method.GetSignature(false));

                    // Rethrow the original exception.
                    throw;
                }
            }

            _log.Debug("Done generating script alias code.");
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

        private static void ImportNamespaces(IScriptSession session, MethodInfo method)
        {
            // Import the method's namespace to the session.
            var @namespace = method.GetNamespace();
            session.ImportNamespace(@namespace);

            // Find out if the method want us to import more namespaces.
            var namespaceAttributes = method.GetCustomAttributes<CakeNamespaceImportAttribute>();
            foreach (var namespaceAttribute in namespaceAttributes)
            {
                session.ImportNamespace(namespaceAttribute.Namespace);
            }

            // Find out if the class contains any more namespaces.
            namespaceAttributes = method.DeclaringType.GetCustomAttributes<CakeNamespaceImportAttribute>();
            foreach (var namespaceAttribute in namespaceAttributes)
            {
                session.ImportNamespace(namespaceAttribute.Namespace);
            }
        }
    }
}
