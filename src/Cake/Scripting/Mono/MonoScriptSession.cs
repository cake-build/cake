using System;
using Mono.CSharp;
using Cake.Core.Scripting;
using System.Text;
using System.Collections.Generic;
using Cake.Scripting.Roslyn;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using System.Linq;
using Cake.Core;

namespace Cake.Scripting.Mono
{
    public class MonoScriptSession : IScriptSession
    {
        
        CompilerSettings compilerSettings;
        ConsoleReportPrinter reportPrinter;
        CompilerContext compilerContext;

        readonly Evaluator evaluator;
        readonly ICakeLog _log;

        readonly string[] SKIP_ASSEMBLIES = {
            "mscorlib",
            "System",
            "System.Core"
        };

        public MonoScriptSession (IScriptHost host, ICakeLog log)
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

            compilerSettings = new CompilerSettings ();

            reportPrinter = new ConsoleReportPrinter ();
            compilerContext = new CompilerContext (compilerSettings, reportPrinter);
            evaluator = new Evaluator (compilerContext);

            // Set our instance of the script host to this static member
            MonoScriptHostProxy.ScriptHost = host;

            // This will be our 'base' type from which the evaluator grants access
            // to static members to the script being run 
            evaluator.InteractiveBaseClass = typeof(MonoScriptHostProxy);
        }

        public void AddReference (FilePath path)
        {
            _log.Debug("Adding reference to {0}...", path.FullPath);
            evaluator.ReferenceAssembly (System.Reflection.Assembly.LoadFile (path.FullPath));
        }

        public void AddReference (System.Reflection.Assembly assembly)
        {
            var name = assembly.GetName ().Name;

            // We don't need to load these ones as they will already get loaded by Mono.CSharp
            if (SKIP_ASSEMBLIES.Contains (name))
                return;
            
            _log.Debug("Adding reference to {0}...", new FilePath(assembly.Location).GetFilename().FullPath);
            evaluator.ReferenceAssembly (assembly);
        }

        public void ImportNamespace (string @namespace)
        {
            _log.Debug("Importing namespace {0}...", @namespace);
            evaluator.Run ("using " + @namespace + ";");
        }

        public void Execute (Script script)
        {
            var generator = new MonoCodeGenerator();
            var code = generator.Generate(script);

            _log.Debug("Compiling build script...");

            // Build the class we generated
            evaluator.Run (code);
            // Actually execute it
            evaluator.Run ("new CakeBuildScriptImpl (ScriptHost).Execute ();");

            _log.Debug ("Execution complete...");
        }            
    }
}

