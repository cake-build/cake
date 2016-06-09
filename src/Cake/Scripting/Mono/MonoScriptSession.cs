// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Linq;
using System.Reflection;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Scripting;
using Cake.Scripting.Mono.CodeGen;
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
            var reportPrinter = new MonoConsoleReportPrinter(_log);
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
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            _log.Debug("Adding reference to {0}...", path.FullPath);
            _evaluator.ReferenceAssembly(Assembly.LoadFrom(path.FullPath));
        }

        public void AddReference(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }

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
            if (script == null)
            {
                throw new ArgumentNullException("script");
            }

            if (script.UsingAliasDirectives.Count > 0)
            {
                throw new CakeException("The Mono scripting engine do not support using alias directives.");
            }

            var code = MonoCodeGenerator.Generate(script);

            try
            {
                // Build the class we generated.
                _log.Verbose("Compiling build script...");
                _evaluator.Run(code);

                // Actually execute it.
                _evaluator.Run("new CakeBuildScriptImpl (ScriptHost).Execute ();");
            }
            catch (InternalErrorException)
            {
                // The error will be logged via the report printer.
                throw new CakeException("An error occured while executing build script.");
            }
        }
    }
}
