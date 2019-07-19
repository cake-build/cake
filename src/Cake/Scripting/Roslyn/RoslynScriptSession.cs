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

        public bool SupportsCachedExecution => true;

        public bool IsCacheValid => System.IO.File.Exists(GetCachedAssemblyPath()) && !_options.ForceRecompile;

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
            if (!string.IsNullOrWhiteSpace(@namespace) && !Namespaces.Contains(@namespace))
            {
                _log.Debug("Importing namespace {0}...", @namespace);
                Namespaces.Add(@namespace);
            }
        }

        private string GetCachedAssemblyPath()
        {
            var cakeAsm = System.Reflection.Assembly.GetExecutingAssembly();
            var cakeAsmDir = System.IO.Path.GetDirectoryName(cakeAsm.Location);
            var directory = System.IO.Path.Combine(cakeAsmDir, $".cache");
            return System.IO.Path.Combine(directory, "./script.dll");
        }

        public void Execute(Script script)
        {
            var assemblyPath = GetCachedAssemblyPath();
            var directory = System.IO.Path.GetDirectoryName(assemblyPath);

            if (_options.ForceRecompile)
            {
                _log.Verbose("--recompile option detected. Recompilation will forced.");
            }
            else
            {
                _log.Verbose("Checking for cached cake assembly in " + assemblyPath);
                if (System.IO.File.Exists(assemblyPath))
                {
                    RunScriptAssembly(assemblyPath);
                    return;
                }
            }

            System.IO.Directory.CreateDirectory(directory);

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
            var emitResult = compilation.Emit(assemblyPath);

            if (emitResult.Success)
            {
                foreach (var r in References)
                {
                    var referenceFileName = System.IO.Path.GetFileName(r.Location);
                    var target = System.IO.Path.Combine(directory, referenceFileName);
                    _log.Verbose($"Copying reference ${referenceFileName} to cache directory.");
                    _log.Verbose($" * {r.Location} => {target}");
                    System.IO.File.Copy(r.Location, target);
                }
                _log.Verbose("Cake assembly cached in " + assemblyPath);
                RunScriptAssembly(assemblyPath);
                return;
            }

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
        }

        private void RunScriptAssembly(string assemblyPath)
        {
            _log.Verbose("Running cached assembly from " + assemblyPath);
            var directory = System.IO.Path.GetDirectoryName(assemblyPath);
            foreach (var asm in System.IO.Directory.GetFiles(directory))
            {
                if (asm.EndsWith(".dll"))
                {
                    try
                    {
                        Assembly.LoadFile(asm);
                    }
                    catch (Exception e)
                    {
                        // usually this is because the assembly is not a proper dotnet assembly,
                        // but was required as a reference to build the script
                        _log.Verbose($"Error when loading {asm} into app domain: {e.ToString()}");
                    }
                }
            }
            var assembly = Assembly.LoadFile(assemblyPath);
            var type = assembly.GetType("Submission#0");
            var factoryMethod = type.GetMethod("<Factory>", new[] { typeof(object[]) });
            using (new ScriptAssemblyResolver(_log))
            {
                try
                {
                    var task = (System.Threading.Tasks.Task<object>)factoryMethod.Invoke(null, new object[] { new object[] { _host, null } });
                    task.Wait();
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.InnerException;
                }
            }
        }
    }
}