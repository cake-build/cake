// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Reflection;
using Cake.Core.Scripting.Analysis;

namespace Cake.Core.Scripting
{
    /// <summary>
    /// Responsible for running scripts.
    /// </summary>
    public sealed class ScriptRunner : IScriptRunner
    {
        private readonly ICakeEnvironment _environment;
        private readonly ICakeLog _log;
        private readonly ICakeConfiguration _configuration;
        private readonly IScriptEngine _engine;
        private readonly IScriptAliasFinder _aliasFinder;
        private readonly IScriptAnalyzer _analyzer;
        private readonly IScriptProcessor _processor;
        private readonly IScriptConventions _conventions;
        private readonly IAssemblyLoader _assemblyLoader;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptRunner"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="log">The log.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="engine">The session factory.</param>
        /// <param name="aliasFinder">The alias finder.</param>
        /// <param name="analyzer">The script analyzer.</param>
        /// <param name="processor">The script processor.</param>
        /// <param name="conventions">The script conventions.</param>
        /// <param name="assemblyLoader">The assembly loader.</param>
        public ScriptRunner(
            ICakeEnvironment environment,
            ICakeLog log,
            ICakeConfiguration configuration,
            IScriptEngine engine,
            IScriptAliasFinder aliasFinder,
            IScriptAnalyzer analyzer,
            IScriptProcessor processor,
            IScriptConventions conventions,
            IAssemblyLoader assemblyLoader)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _engine = engine ?? throw new ArgumentNullException(nameof(engine));
            _aliasFinder = aliasFinder ?? throw new ArgumentNullException(nameof(aliasFinder));
            _analyzer = analyzer ?? throw new ArgumentNullException(nameof(analyzer));
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
            _conventions = conventions ?? throw new ArgumentNullException(nameof(conventions));
            _assemblyLoader = assemblyLoader ?? throw new ArgumentNullException(nameof(assemblyLoader));
        }

        /// <inheritdoc/>
        public void Run(IScriptHost host, FilePath scriptPath)
        {
            if (host == null)
            {
                throw new ArgumentNullException(nameof(host));
            }
            if (scriptPath == null)
            {
                throw new ArgumentNullException(nameof(scriptPath));
            }

            // Make the script path absolute.
            scriptPath = scriptPath.MakeAbsolute(_environment);

            // Prepare the environment.
            _environment.WorkingDirectory = scriptPath.GetDirectory();

            // Analyze the script file.
            _log.Verbose("Analyzing build script...");
            var result = _analyzer.Analyze(scriptPath.GetFilename(), new ScriptAnalyzerSettings());

            // Log all errors and throw
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    var format = $"{error.File.MakeAbsolute(_environment).FullPath}:{error.Line}: {{0}}";
                    _log.Error(format, error.Message);
                }
                throw new CakeException("Errors occurred while analyzing script.");
            }

            // Install tools.
            _log.Verbose("Processing build script...");
            var toolsPath = GetToolPath(scriptPath.GetDirectory());
            _processor.InstallTools(result.Tools, toolsPath);

            // Install addins.
            var addinRoot = GetAddinPath(scriptPath.GetDirectory());
            var addinReferences = _processor.InstallAddins(result.Addins, addinRoot);
            foreach (var addinReference in addinReferences)
            {
                result.References.Add(addinReference.FullPath);
            }

            // Create and prepare the session.
            var session = _engine.CreateSession(host);

            // Load all references.
            var applicationRoot = _environment.ApplicationRoot;
            var assemblies = new HashSet<Assembly>();
            assemblies.AddRange(_conventions.GetDefaultAssemblies(applicationRoot));

            foreach (var reference in result.References)
            {
                var referencePath = new FilePath(reference);
                if (host.Context.FileSystem.Exist(referencePath))
                {
                    var assembly = _assemblyLoader.Load(referencePath, true);

                    if (assembly != null)
                    {
                        assemblies.Add(assembly);
                    }
                }
                else
                {
                    // Add a reference to the session.
                    session.AddReference(referencePath);
                }
            }

            var aliases = new List<ScriptAlias>();

            // Got any assemblies?
            if (assemblies.Count > 0)
            {
                // Find all script aliases.
                var foundAliases = _aliasFinder.FindAliases(assemblies);
                if (foundAliases.Count > 0)
                {
                    aliases.AddRange(foundAliases);
                }

                // Add assembly references to the session.
                foreach (var assembly in assemblies)
                {
                    session.AddReference(assembly);
                }
            }

            // Import all namespaces.
            var namespaces = new HashSet<string>(result.Namespaces, StringComparer.Ordinal);
            namespaces.AddRange(_conventions.GetDefaultNamespaces());
            namespaces.AddRange(aliases.SelectMany(alias => alias.Namespaces));
            foreach (var @namespace in namespaces.OrderBy(ns => ns))
            {
                session.ImportNamespace(@namespace);
            }

            var defines = new HashSet<string>(result.Defines, StringComparer.Ordinal);
            defines.AddRange(_conventions.GetDefaultDefines());

            // Execute the script.
            var script = new Script(result.Namespaces, result.Lines, aliases, result.UsingAliases, result.UsingStaticDirectives, defines);
            session.Execute(script);
        }

        private DirectoryPath GetToolPath(DirectoryPath root)
        {
            var toolPath = _configuration.GetValue(Constants.Paths.Tools);
            if (!string.IsNullOrWhiteSpace(toolPath))
            {
                return new DirectoryPath(toolPath).MakeAbsolute(_environment);
            }

            return root.Combine("tools");
        }

        private DirectoryPath GetAddinPath(DirectoryPath root)
        {
            var addinPath = _configuration.GetValue(Constants.Paths.Addins);
            if (!string.IsNullOrWhiteSpace(addinPath))
            {
                return new DirectoryPath(addinPath).MakeAbsolute(_environment);
            }

            var toolPath = GetToolPath(root);
            return toolPath.Combine("Addins").Collapse();
        }
    }
}