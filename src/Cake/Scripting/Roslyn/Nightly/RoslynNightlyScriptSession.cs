using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Scripting;

namespace Cake.Scripting.Roslyn.Nightly
{
    internal sealed class RoslynNightlyScriptSession : IScriptSession
    {
        private readonly IScriptHost _host;
        private readonly ICakeLog _log;
        private readonly HashSet<FilePath> _referencePaths;
        private readonly HashSet<Assembly> _references;
        private readonly HashSet<string> _namespaces;

        public RoslynNightlyScriptSession(IScriptHost host, ICakeLog log)
        {
            _host = host;
            _log = log;

            _referencePaths = new HashSet<FilePath>(PathComparer.Default);
            _references = new HashSet<Assembly>();
            _namespaces = new HashSet<string>(StringComparer.Ordinal);
        }

        public void AddReference(FilePath path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            _log.Debug("Adding reference to {0}...", path.GetFilename().FullPath);
            _referencePaths.Add(path);
        }

        public void AddReference(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }
            _log.Debug("Adding reference to {0}...", new FilePath(assembly.Location).GetFilename().FullPath);
            _references.Add(assembly);
        }

        public void ImportNamespace(string @namespace)
        {
            if (!_namespaces.Contains(@namespace))
            {
                _log.Debug("Importing namespace {0}...", @namespace);
                _namespaces.Add(@namespace);
            }
        }

        public void Execute(Script script)
        {
            // Generate the script code.
            var generator = new RoslynCodeGenerator();
            var code = generator.Generate(script);

            // Create the script options dynamically.
            var options = Microsoft.CodeAnalysis.Scripting.ScriptOptions.Default
                .AddNamespaces(_namespaces)
                .AddReferences(_references)
                .AddReferences(_referencePaths.Select(r => r.FullPath));

            _log.Verbose("Compiling build script...");
            Microsoft.CodeAnalysis.Scripting.CSharp.CSharpScript.Eval(code, options, _host);
        }
    }
}