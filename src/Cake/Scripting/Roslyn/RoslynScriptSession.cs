// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Reflection;
using Cake.Core.Scripting;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace Cake.Scripting.Roslyn
{
    internal sealed class RoslynScriptSession : IScriptSession
    {
        private readonly IScriptHost _host;
        private readonly IAssemblyLoader _loader;
        private readonly ICakeLog _log;
        private readonly CakeOptions _options;

        public HashSet<FilePath> ReferencePaths { get; }

        public HashSet<Assembly> References { get; }

        public HashSet<string> Namespaces { get; }

        public RoslynScriptSession(IScriptHost host, IAssemblyLoader loader, ICakeLog log, CakeOptions options)
        {
            _host = host;
            _loader = loader;
            _log = log;
            _options = options;

            ReferencePaths = new HashSet<FilePath>(PathComparer.Default);
            References = new HashSet<Assembly>();
            Namespaces = new HashSet<string>(StringComparer.Ordinal);
        }

        public void AddReference(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }
            _log.Debug("Adding assembly reference to {0}...", new FilePath(assembly.Location).GetFilename().FullPath);
            References.Add(assembly);
        }

        public void AddReference(FilePath path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            _log.Debug("Adding reference to {0}...", path.GetFilename().FullPath);
#if NETCORE
            References.Add(_loader.Load(path, true));
#else
            ReferencePaths.Add(path);
#endif
        }

        public void ImportNamespace(string @namespace)
        {
            if (!Namespaces.Contains(@namespace))
            {
                _log.Debug("Importing namespace {0}...", @namespace);
                Namespaces.Add(@namespace);
            }
        }

        public void Execute(Script script)
        {
            // Generate the script code.
            var generator = new RoslynCodeGenerator();
            var code = generator.Generate(script);

            // Create the script options dynamically.
            var options = Microsoft.CodeAnalysis.Scripting.ScriptOptions.Default
                .AddImports(Namespaces)
                .AddReferences(References)
                .AddReferences(ReferencePaths.Select(r => r.FullPath))
                .WithEmitDebugInformation(_options.PerformDebug)
                .WithMetadataResolver(Microsoft.CodeAnalysis.Scripting.ScriptMetadataResolver.Default);

            var roslynScript = CSharpScript.Create(code, options, _host.GetType());

            _log.Verbose("Compiling build script...");
            var compilation = roslynScript.GetCompilation();
            var diagnostics = compilation.GetDiagnostics();

            if (diagnostics.Any(d => d.Severity == DiagnosticSeverity.Error))
            {
                var errors = string.Join(Environment.NewLine, diagnostics.Select(x => x.ToString()));
                var message = string.Format(CultureInfo.InvariantCulture, "Error occurred when compiling build script: {0}", errors);
                throw new CakeException(message);
            }

            roslynScript.RunAsync(_host).Wait();
        }
    }
}