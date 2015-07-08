using System;
using System.Linq;
using System.Reflection;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Scripting;
using Mono.CSharp;

namespace Cake.Scripting.Mono
{
    internal sealed class MonoScriptSession : IScriptSession
    {
        private readonly Evaluator _evaluator;
        private readonly ICakeLog _log;

        private readonly string[] _skipAssemblies = 
        {
            "mscorlib",
            "System",
            "System.Core"
        };

        public MonoScriptSession(IScriptHost host, ICakeLog log)
        {
            if (host == null)
            {
                throw new ArgumentNullException("host");
            }
            if (log == null)
            {
                throw new ArgumentNullException("log");
            }

            _log = log;

            // Create the evaluator.
            var compilerSettings = new CompilerSettings();
            var reportPrinter = new ConsoleReportPrinter();
            var compilerContext = new CompilerContext(compilerSettings, reportPrinter);
            _evaluator = new Evaluator(compilerContext);

            // Set our instance of the script host to this static member
            MonoScriptHostProxy.ScriptHost = host;

            // This will be our 'base' type from which the evaluator grants access
            // to static members to the script being run 
            _evaluator.InteractiveBaseClass = typeof(MonoScriptHostProxy);
        }

        public void AddReference(FilePath path)
        {
            _log.Debug("Adding reference to {0}...", path.FullPath);
            _evaluator.ReferenceAssembly(Assembly.LoadFile(path.FullPath));
        }

        public void AddReference(Assembly assembly)
        {
            var name = assembly.GetName().Name;

            // We don't need to load these ones as they will already get loaded by Mono.CSharp
            if (_skipAssemblies.Contains(name))
            {
                return;
            }

            _log.Debug("Adding reference to {0}...", new FilePath(assembly.Location).GetFilename().FullPath);
            _evaluator.ReferenceAssembly(assembly);
        }

        public void ImportNamespace(string @namespace)
        {
            _log.Debug("Importing namespace {0}...", @namespace);
            _evaluator.Run("using " + @namespace + ";");
        }

        public void Execute(Script script)
        {
            var generator = new MonoCodeGenerator();
            
            int codeLineOffset;
            var code = generator.Generate(script, out codeLineOffset);

            _log.Debug("Compiling build script...");

            _log.Information(Verbosity.Normal, "User Code Starts at Line # {0}", codeLineOffset);

            // Build the class we generated
            _evaluator.Run(code);

            // Actually execute it
            _evaluator.Run("new CakeBuildScriptImpl (ScriptHost).Execute ();");

            _log.Debug("Execution complete...");
        }
    }
}
