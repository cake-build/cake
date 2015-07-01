using System;
using Mono.CSharp;
using Cake.Core.Scripting;
using System.Text;
using System.Collections.Generic;
using Cake.Scripting.Roslyn;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Scripting.Mono
{
    public class MonoScriptSession : IScriptSession
    {
        Evaluator evaluator;
        CompilerSettings compilerSettings;
        ConsoleReportPrinter reportPrinter;
        CompilerContext compilerContext;

        readonly ICakeLog _log;

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

            reportPrinter = new ConsoleReportPrinter ();// CakeReportPrinter (new CakeConsole ());
            compilerContext = new CompilerContext (compilerSettings, reportPrinter);
            evaluator = new Evaluator (compilerContext);

            // Set our instance of the script host to this static member
            MonoScriptHostProxy.ScriptHost = host;

            // This will be our 'base' type from which the evaluator grants access
            // to static members to the script being run 
            evaluator.ReferenceAssembly(typeof(MonoScriptHostProxy).Assembly);
            evaluator.InteractiveBaseClass = typeof(MonoScriptHostProxy);
        }

        public void AddReference (Cake.Core.IO.FilePath path)
        {
            _log.Debug("Adding reference to {0}...", path.FullPath);
            evaluator.ReferenceAssembly (System.Reflection.Assembly.LoadFile (path.FullPath));
        }

        public void AddReference (System.Reflection.Assembly assembly)
        {
            _log.Debug("Adding reference to {0}...", new FilePath(assembly.Location).GetFilename().FullPath);
            evaluator.ReferenceAssembly (assembly);
        }

        public void ImportNamespace (string @namespace)
        {
            _log.Debug("Importing namespace {0}...", @namespace);
            var result = evaluator.Run ("using " + @namespace + ";");
            _log.Debug ("Ran Import {0}...", result);
        }

        public void Execute (Script script)
        {
            var generator = new MonoCodeGenerator();
            var code = generator.Generate(script);

            var result = evaluator.Evaluate (code);

            _log.Debug ("Execution complete");
        }            
    }
}

