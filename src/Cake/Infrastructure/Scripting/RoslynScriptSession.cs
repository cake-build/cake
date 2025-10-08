// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Cake.Core;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Reflection;
using Cake.Core.Scripting;
using Cake.Infrastructure.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace Cake.Infrastructure.Scripting
{
    /// <summary>
    /// Represents a Roslyn-based script session for Cake.
    /// </summary>
    public sealed class RoslynScriptSession : IScriptSession
    {
        private readonly IScriptHost _host;
        private readonly IFileSystem _fileSystem;
        private readonly IAssemblyLoader _loader;
        private readonly ICakeLog _log;
        private readonly ICakeConfiguration _configuration;
        private readonly IScriptHostSettings _settings;

        private readonly bool _scriptCacheEnabled;
        private readonly bool _regenerateCache;
        private readonly DirectoryPath _scriptCachePath;

        /// <summary>
        /// Gets the reference paths.
        /// </summary>
        public HashSet<FilePath> ReferencePaths { get; }

        /// <summary>
        /// Gets the references.
        /// </summary>
        public HashSet<Assembly> References { get; }

        /// <summary>
        /// Gets the namespaces.
        /// </summary>
        public HashSet<string> Namespaces { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoslynScriptSession"/> class.
        /// </summary>
        /// <param name="host">The script host.</param>
        /// <param name="loader">The assembly loader.</param>
        /// <param name="configuration">The Cake configuration.</param>
        /// <param name="log">The log.</param>
        /// <param name="settings">The script host settings.</param>
        public RoslynScriptSession(
            IScriptHost host,
            IAssemblyLoader loader,
            ICakeConfiguration configuration,
            ICakeLog log,
            IScriptHostSettings settings)
        {
            _host = host;
            _fileSystem = host.Context.FileSystem;
            _loader = loader;
            _log = log;
            _configuration = configuration;
            _settings = settings;

            ReferencePaths = new HashSet<FilePath>(PathComparer.Default);
            References = new HashSet<Assembly>();
            Namespaces = new HashSet<string>(StringComparer.Ordinal);

            var cacheEnabled = configuration.GetValue(Constants.Settings.EnableScriptCache) ?? bool.FalseString;
            _scriptCacheEnabled = cacheEnabled.Equals(bool.TrueString, StringComparison.OrdinalIgnoreCase);
            _regenerateCache = host.Context.Arguments.HasArgument(Constants.Cache.InvalidateScriptCache);
            _scriptCachePath = configuration.GetScriptCachePath(settings.Script.GetDirectory(), host.Context.Environment);
        }

        /// <summary>
        /// Adds an assembly reference.
        /// </summary>
        /// <param name="assembly">The assembly to add.</param>
        public void AddReference(Assembly assembly)
        {
            ArgumentNullException.ThrowIfNull(assembly);
            _log.Debug("Adding assembly reference to {0}...", new FilePath(assembly.Location).GetFilename().FullPath);
            References.Add(assembly);
        }

        /// <summary>
        /// Adds a reference from the specified file path.
        /// </summary>
        /// <param name="path">The file path to add as a reference.</param>
        public void AddReference(FilePath path)
        {
            ArgumentNullException.ThrowIfNull(path);

            _log.Debug("Adding reference to {0}...", path.GetFilename().FullPath);
            References.Add(_loader.Load(path, true));
        }

        /// <summary>
        /// Imports a namespace.
        /// </summary>
        /// <param name="namespace">The namespace to import.</param>
        public void ImportNamespace(string @namespace)
        {
            if (!string.IsNullOrWhiteSpace(@namespace) && !Namespaces.Contains(@namespace))
            {
                _log.Debug("Importing namespace {0}...", @namespace);
                Namespaces.Add(@namespace);
            }
        }

        /// <summary>
        /// Executes the specified script.
        /// </summary>
        /// <param name="script">The script to execute.</param>
        public void Execute(Script script)
        {
            var scriptName = _settings.Script.GetFilename();
            FilePath cachedAssembly = _scriptCacheEnabled
                                        ? GetCachedAssemblyPath(script, scriptName)
                                        : default;

            if (_scriptCacheEnabled && _fileSystem.Exist(cachedAssembly) && !_regenerateCache)
            {
                _log.Verbose("Running cached build script...");
                RunScriptAssembly(cachedAssembly.FullPath);
                return;
            }

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
#if NET10_0
                .WithLanguageVersion(Microsoft.CodeAnalysis.CSharp.LanguageVersion.Preview)
#endif
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
                // Suppress some diagnostic information. See https://github.com/cake-build/cake/issues/3337
                switch (diagnostic.Id)
                {
                    // CS1701 Compiler Warning (level 2)
                    // Assuming assembly reference "Assembly Name #1" matches "Assembly Name #2", you may need to supply runtime policy
                    // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/cs1701
                    case "CS1701":
                        continue;

                    // CS1702 Compiler Warning (level 3)
                    // Assuming assembly reference "Assembly Name #1" matches "Assembly Name #2", you may need to supply runtime policy
                    // https://docs.microsoft.com/en-us/dotnet/csharp/misc/cs1702
                    case "CS1702":
                        continue;

                    // CS1705 Compiler Error
                    // Assembly 'AssemblyName1' uses 'TypeName' which has a higher version than referenced assembly 'AssemblyName2'
                    case "CS1705":
                        continue;

                    default:
                        break;
                }

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

            if (_scriptCacheEnabled)
            {
                // Verify cache directory exists
                if (!_fileSystem.GetDirectory(_scriptCachePath).Exists)
                {
                    _fileSystem.GetDirectory(_scriptCachePath).Create();
                }

                var emitResult = compilation.Emit(cachedAssembly.FullPath);

                if (emitResult.Success)
                {
                    RunScriptAssembly(cachedAssembly.FullPath);
                }
            }
            else
            {
                roslynScript.RunAsync(_host).GetAwaiter().GetResult();
            }
        }

        private FilePath GetCachedAssemblyPath(Script script, FilePath scriptName)
            => _scriptCachePath.CombineWithFilePath(
                string.Join(
                    '.',
                    scriptName.GetFilenameWithoutExtension().FullPath,
                    _host.GetType().Name,
                    GetScriptHash(script),
                    "dll"));

        /// <summary>
        /// Gets the hash for the specified script.
        /// </summary>
        /// <param name="script">The script to get the hash for.</param>
        /// <returns>The script hash.</returns>
        public string GetScriptHash(Script script)
        {
            // Remove specific lines that could cause the same files to generate different
            // hashes. See https://github.com/cake-build/cake/issues/4471 for more information
            var linesToHash = script.Lines
                .Where(line => !line.StartsWith("#line ", StringComparison.OrdinalIgnoreCase))
                .ToArray();

            var hash = FastHash.GenerateHash(Encoding.UTF8.GetBytes(string.Concat(linesToHash)));
            return hash;
        }

        private void RunScriptAssembly(string assemblyPath)
        {
            var assembly = _loader.Load(assemblyPath, false);
            var type = assembly.GetType("Submission#0");
            var factoryMethod = type.GetMethod("<Factory>", new[] { typeof(object[]) });
            var task = (Task<object>)factoryMethod.Invoke(null, new object[] { new object[] { _host, null } });
            task.GetAwaiter().GetResult();
        }
    }
}