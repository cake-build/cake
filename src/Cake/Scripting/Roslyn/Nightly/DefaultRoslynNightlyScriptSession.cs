using System;
using System.Linq;
using Cake.Core.Diagnostics;
using Cake.Core.Scripting;

namespace Cake.Scripting.Roslyn.Nightly
{
    internal sealed class DefaultRoslynNightlyScriptSession : RoslynNightlyScriptSession
    {
        private readonly IScriptHost _host;
        private readonly ICakeLog _log;

        public DefaultRoslynNightlyScriptSession(IScriptHost host, ICakeLog log) : base(log)
        {
            if (host == null)
            {
                throw new ArgumentNullException("host");
            }
            if (log == null)
            {
                throw new ArgumentNullException("log");
            }

            _host = host;
            _log = log;
        }

        public override void Execute(Script script)
        {
            // Generate the script code.
            var generator = new RoslynCodeGenerator();
            var code = generator.Generate(script);

            // Create the script options dynamically.
            var options = Microsoft.CodeAnalysis.Scripting.ScriptOptions.Default
                .AddNamespaces(Namespaces)
                .AddReferences(References)
                .AddReferences(ReferencePaths.Select(r => r.FullPath));

            _log.Verbose("Compiling build script...");
            Microsoft.CodeAnalysis.Scripting.CSharp.CSharpScript.Eval(code, options, _host);
        }
    }
}