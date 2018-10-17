// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Cake.Core;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Reflection;
using Cake.Core.Scripting;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace Cake.Infrastructure.Scripting
{
    public sealed class RoslynScriptSession : IScriptSession
    {
        private readonly IScriptHost _host;
        private readonly IAssemblyLoader _loader;
        private readonly ICakeLog _log;
        private readonly ICakeConfiguration _configuration;
        private readonly IScriptHostSettings _settings;

        public HashSet<FilePath> ReferencePaths { get; }

        public HashSet<Assembly> References { get; }

        public HashSet<string> Namespaces { get; }

        public RoslynScriptSession(
            IScriptHost host,
            IAssemblyLoader loader,
            ICakeConfiguration configuration,
            ICakeLog log,
            IScriptHostSettings settings)
        {
            _host = host;
            _loader = loader;
            _log = log;
            _configuration = configuration;
            _settings = settings;

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
            if (!string.IsNullOrWhiteSpace(@namespace) && !Namespaces.Contains(@namespace))
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

            // Warn about any code generation excluded namespaces
            foreach (var @namespace in script.ExcludedNamespaces)
            {
                _log.Warning("Namespace {0} excluded by code generation, affected methods:\r\n\t{1}",
                    @namespace.Key, string.Join("\r\n\t", @namespace.Value));
            }

            // Create the script options dynamically.
            var options = Microsoft.CodeAnalysis.Scripting.ScriptOptions.Default
                .AddImports(Namespaces.Except(script.ExcludedNamespaces.Keys))
                .AddReferences(References)
                .AddReferences(ReferencePaths.Select(r => r.FullPath))
                .WithEmitDebugInformation(_settings.Debug)
                .WithMetadataResolver(Microsoft.CodeAnalysis.Scripting.ScriptMetadataResolver.Default);

            var roslynScript = CSharpScript.Create(code, options, _host.GetType());

            _log.Verbose("Compiling build script...");
            var compilation = roslynScript.GetCompilation();
            var diagnostics = compilation.GetDiagnostics();

            var errors = new List<Diagnostic>();

            foreach (var diagnostic in diagnostics)
            {
                switch (diagnostic.Severity)
                {
                    case DiagnosticSeverity.Info:
                        _log.Information(diagnostic.ToString());
                        break;
                    case DiagnosticSeverity.Warning:
                        _log.Warning(diagnostic.ToString());
                        break;
                    case DiagnosticSeverity.Error:
                        _log.Error(diagnostic.ToString());
                        errors.Add(diagnostic);
                        break;
                    default:
                        break;
                }
            }

            if (errors.Any())
            {
                var errorMessages = string.Join(Environment.NewLine, errors.Select(x => x.ToString()));
                var message = string.Format(CultureInfo.InvariantCulture, "Error(s) occurred when compiling build script:{0}{1}", Environment.NewLine, errorMessages);
                throw new CakeException(message);
            }

            roslynScript.RunAsync(_host).Wait();
        }
    }
}