using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cake.Core.Diagnostics;
using Cake.Extensions;
using Cake.Scripting.Host;
using Roslyn.Scripting;
using Roslyn.Scripting.CSharp;

namespace Cake.Scripting
{
    public sealed class ScriptRunner : IScriptRunner
    {
        private readonly ICakeLog _log;

        public ScriptRunner(ICakeLog log)
        {
            _log = log;
        }

        public void Run(ScriptHost host, IEnumerable<Assembly> references, IEnumerable<string> namespaces, string code)
        {
            var script = new ScriptEngine();
            var session = script.CreateSession(host);
            var assemblies = references as Assembly[] ?? references.ToArray();

            // Add references to session.
            foreach (var reference in assemblies)
            {
                session.AddReference(reference);
            }

            // Add namespaces to session.
            foreach (var @namespace in namespaces)
            {
                session.ImportNamespace(@namespace);
            }

            // Find all extension methods and generate proxy methods.
            AddExtensionMethodsToSession(session, assemblies);

            // Execute the code.
            session.Execute(code);
        }

        private void AddExtensionMethodsToSession(Session session, IEnumerable<Assembly> assemblies)
        {
            _log.Debug("Generating script host proxy methods for extension methods...");
            foreach (var method in ScriptExtensionFinder.GetExtensionMethods(assemblies))
            {
                var signature = method.GetSignature();

                try
                {
                    _log.Debug("  {0}", signature);
                    var extensionWrapper = ScriptCodeGenerator.Generate(method);
                    session.Execute(extensionWrapper);
                }
                catch (Exception)
                {
                    _log.Error("An error occured while generating proxy code for {0}", signature);
                    throw;
                }
            }
            _log.Debug("Done generating code.");
        }
    }
}