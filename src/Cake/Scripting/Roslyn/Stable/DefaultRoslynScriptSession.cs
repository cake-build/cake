using System;
using Cake.Core.Diagnostics;
using Cake.Core.Scripting;

namespace Cake.Scripting.Roslyn.Stable
{
    internal sealed class DefaultRoslynScriptSession : RoslynScriptSession
    {
        private readonly ICakeLog _log;

        public DefaultRoslynScriptSession(IScriptHost host, ICakeLog log) : base(host, log)
        {
            if (log == null)
            {
                throw new ArgumentNullException("log");
            }

            _log = log;
        }

        public override void Execute(Script script)
        {
            // Generate the script code.
            var generator = new RoslynCodeGenerator();
            var code = generator.Generate(script);

            _log.Verbose("Compiling build script...");
            RoslynSession.Execute(code);
        }
    }
}