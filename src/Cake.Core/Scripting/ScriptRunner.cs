﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Packaging;
using Cake.Core.Scripting.Analysis;
using Cake.Core.Scripting.Processors;

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
        public ScriptRunner(
            ICakeEnvironment environment,
            ICakeLog log,
            ICakeConfiguration configuration,
            IScriptEngine engine,
            IScriptAliasFinder aliasFinder,
            IScriptAnalyzer analyzer,
            IScriptProcessor processor,
            IScriptConventions conventions)
        {
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }
            if (log == null)
            {
                throw new ArgumentNullException("log");
            }
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }
            if (engine == null)
            {
                throw new ArgumentNullException("engine");
            }
            if (aliasFinder == null)
            {
                throw new ArgumentNullException("aliasFinder");
            }
            if (analyzer == null)
            {
                throw new ArgumentNullException("analyzer");
            }
            if (processor == null)
            {
                throw new ArgumentNullException("processor");
            }
            if (conventions == null)
            {
                throw new ArgumentNullException("conventions");
            }

            _environment = environment;
            _log = log;
            _configuration = configuration;
            _engine = engine;
            _aliasFinder = aliasFinder;
            _analyzer = analyzer;
            _processor = processor;
            _conventions = conventions;
        }

        /// <summary>
        /// Runs the script using the specified script host.
        /// </summary>
        /// <param name="host">The script host.</param>
        /// <param name="scriptPath">The script.</param>
        /// <param name="arguments">The arguments.</param>
        public void Run(IScriptHost host, FilePath scriptPath, IDictionary<string, string> arguments)
        {
            if (host == null)
            {
                throw new ArgumentNullException("host");
            }
            if (scriptPath == null)
            {
                throw new ArgumentNullException("scriptPath");
            }
            if (arguments == null)
            {
                throw new ArgumentNullException("arguments");
            }

            // Make the script path absolute.
            scriptPath = scriptPath.MakeAbsolute(_environment);

            // Prepare the environment.
            _environment.WorkingDirectory = scriptPath.GetDirectory();

            // Analyze the script file.
            _log.Verbose("Analyzing build script...");
            IScriptAnalyzerContext scriptAnalyzerContext;
            var result = _analyzer.Analyze(scriptPath.GetFilename(), out scriptAnalyzerContext);

            // Import nuscripts.
            var nuScriptPath = GetToolPath(scriptPath.GetDirectory());
            IEnumerable<KeyValuePair<PackageReference, FilePath>> scriptImports = _processor.InstallNuScripts(result.NuScripts, nuScriptPath);
            RecursiveInstallNuScripts(ref result, scriptImports, scriptAnalyzerContext, nuScriptPath);

            //foreach (var file in scriptImports)
            //{
            //    scriptAnalyzerContext.Analyze(file);
            //    var nuScriptResult = new ScriptAnalyzerResult(scriptAnalyzerContext.Script, scriptAnalyzerContext.Lines);

            //    // add all tools, addins, namespaces etc from nuScriptResult to result.
            //    result.Tools.AddRange(nuScriptResult.Tools);
            //    result.Addins.AddRange(nuScriptResult.Addins);
            //    foreach (var reference in nuScriptResult.References)
            //    {
            //        result.References.Add(reference);
            //    }

            //    foreach (var @namespace in nuScriptResult.Namespaces)
            //    {
            //        result.Namespaces.Add(@namespace);
            //    }
            //}

            // Install tools.
            _log.Verbose("Processing build script...");
            var toolsPath = GetToolPath(scriptPath.GetDirectory());
            _processor.InstallTools(result, toolsPath);

            // Install addins.
            var applicationRoot = _environment.ApplicationRoot;
            var addinRoot = GetAddinPath(applicationRoot);
            var addinReferences = _processor.InstallAddins(result, addinRoot);
            foreach (var addinReference in addinReferences)
            {
                result.References.Add(addinReference.FullPath);
            }

            // Create and prepare the session.
            var session = _engine.CreateSession(host, arguments);

            // Load all references.
            var assemblies = new HashSet<Assembly>();
            assemblies.AddRange(_conventions.GetDefaultAssemblies(applicationRoot));
            foreach (var reference in result.References)
            {
                if (host.Context.FileSystem.Exist((FilePath)reference))
                {
                    var assembly = Assembly.LoadFrom(reference);
                    assemblies.Add(assembly);
                }
                else
                {
                    // Add a reference to the session.
                    session.AddReference(reference);
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
            
            // Execute the script.
            var script = new Script(result.Namespaces, result.Lines, aliases, result.UsingAliases);
            session.Execute(script);
        }

        private void RecursiveInstallNuScripts(ref ScriptAnalyzerResult result, IEnumerable<KeyValuePair<PackageReference, FilePath>> scriptImports, IScriptAnalyzerContext scriptAnalyzerContext, DirectoryPath nuScriptPath)
        {
            foreach (var item in scriptImports)
            {
                var file = item.Value;
                scriptAnalyzerContext.Analyze(file);
                var nuScriptResult = new ScriptAnalyzerResult(scriptAnalyzerContext.Script, scriptAnalyzerContext.Lines);

                // add all tools, addins, namespaces etc from nuScriptResult to result.
                result.Tools.AddRange(nuScriptResult.Tools);
                result.Addins.AddRange(nuScriptResult.Addins);
                foreach (var reference in nuScriptResult.References)
                {
                    result.References.Add(reference);
                }

                foreach (var @namespace in nuScriptResult.Namespaces)
                {
                    result.Namespaces.Add(@namespace);
                }

                var childScripts = _processor.InstallNuScripts(nuScriptResult.NuScripts, nuScriptPath).ToList();
                if (childScripts.Any())
                {
                    RecursiveInstallNuScripts(ref result, childScripts, scriptAnalyzerContext, nuScriptPath);
                }
                
                // Re Arange the nuscript to its rightfull place
                var lineCopy = result.Lines.ToList();
                var lineMarker = result.Lines.FirstOrDefault(x => x.StartsWith("#line") && x.Contains(file.FullPath));
                if (lineMarker != null)
                {
                    var startIndex = lineCopy.IndexOf(lineMarker);
                    var prevLineDirective = result.Lines.LastOrDefault(x => x.StartsWith("#line") && !x.Contains(file.FullPath));
                    
                    // Read all of the lines belonging to the imported script
                    var amountToTake = lineCopy.Count - startIndex;
                    var nuScriptLines = lineCopy.GetRange(startIndex, amountToTake);

                    // Remove the copied lines
                    lineCopy.RemoveRange(startIndex, amountToTake);

                    // Get the nuscript directive declaration index
                    var nuscriptMarker = lineCopy.FirstOrDefault(x => x.Contains(NuScriptDirectiveProcessor.DirectiveName) && x.Contains(item.Key.OriginalString));
                    var nuscriptIndex = lineCopy.IndexOf(nuscriptMarker) + 1;

                    // Performe the actuall move of the imported script
                    lineCopy.InsertRange(nuscriptIndex, nuScriptLines);

                    // Add the previus #line marker back
                    var lineDirectiveIndex = nuscriptIndex + nuScriptLines.Count;

                    if (prevLineDirective != null)
                    {
                        var prevLine = prevLineDirective.Split(null);
                        var prevLineDirectiveIndex = lineCopy.IndexOf(prevLineDirective);

                        // Calculate the new line number for the previus #line directive. (note: we need to do -1 becuse of line 270 nuscriptIndex has +1)
                        var calculateFromBegining = lineCopy.Skip(prevLineDirectiveIndex).TakeWhile(x => !x.Equals(lineMarker)).Count() - 1;

                        // Ensure a minimum number 1
                        calculateFromBegining = Math.Max(1, calculateFromBegining);

                        // Set the line number
                        prevLine[1] = calculateFromBegining + string.Empty;

                        var newValue = string.Join(" ", prevLine);

                        // Add a new #line directive with a new line number
                        lineCopy.Insert(lineDirectiveIndex, newValue);

                        // Check if the line directive is the last line and if so remove (im sure there is much better ways to do this but im tired)
                        var lastItem = lineCopy.Last();
                        if (lastItem.Equals(newValue))
                        {
                            lineCopy.RemoveAt(lineCopy.Count - 1);
                        }
                    }

                    // Persist changes
                    result = new ScriptAnalyzerResult(result.Script, lineCopy);
                }
            }
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

        private DirectoryPath GetAddinPath(DirectoryPath applicationRoot)
        {
            var addinPath = _configuration.GetValue(Constants.Paths.Addins);
            if (!string.IsNullOrWhiteSpace(addinPath))
            {
                return new DirectoryPath(addinPath).MakeAbsolute(_environment);
            }

            return applicationRoot.Combine("../Addins").Collapse();
        }
    }
}
