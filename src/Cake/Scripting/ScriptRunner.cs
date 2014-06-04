using System.Collections.Generic;
using System.Reflection;
using Roslyn.Scripting.CSharp;

namespace Cake.Scripting
{
    public sealed class ScriptRunner : IScriptRunner
    {
        public void Run(ScriptHost host, IEnumerable<Assembly> references, IEnumerable<string> namespaces, string code)
        {
            var script = new ScriptEngine();
            var session = script.CreateSession(host);

            // Add references to session.
            foreach (var reference in references)
            {
                session.AddReference(reference);
            }

            // Add namespaces to session.
            foreach (var @namespace in namespaces)
            {
                session.ImportNamespace(@namespace);
            }

            // Execute the code.
            session.Execute(code);
        }
    }
}