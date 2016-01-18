using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
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
        /// <param name="engine">The session factory.</param>
        /// <param name="aliasFinder">The alias finder.</param>
        /// <param name="analyzer">The script analyzer.</param>
        /// <param name="processor">The script processor.</param>
        /// <param name="conventions">The script conventions.</param>
        public ScriptRunner(
            ICakeEnvironment environment,
            ICakeLog log,
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

            // Prepare the context.
            host.Context.Arguments.SetArguments(arguments);
            host.Context.Environment.WorkingDirectory = scriptPath.MakeAbsolute(host.Context.Environment).GetDirectory();

            // Analyze the script file.
            _log.Verbose("Analyzing build script...");
            scriptPath = scriptPath.IsRelative ? scriptPath.MakeAbsolute(_environment) : scriptPath;
            var result = _analyzer.Analyze(scriptPath.GetFilename());

            // Install tools.
            _log.Verbose("Processing build script...");
            var toolsEnv = _environment.GetEnvironmentVariable("CAKE_TOOLS");

            DirectoryPath toolsPath;
            if (!string.IsNullOrEmpty(toolsEnv))
            {
                toolsPath = new DirectoryPath(toolsEnv).MakeAbsolute(_environment);
            }
            else
            {
                toolsPath = scriptPath.GetDirectory().Combine("tools");
            }

            _processor.InstallTools(result, toolsPath);

            // Install addins.
            var applicationRoot = _environment.GetApplicationRoot();
            var addinsEnv = _environment.GetEnvironmentVariable("CAKE_ADDINS");

            DirectoryPath addinPath;
            if (!string.IsNullOrEmpty(addinsEnv))
            {
                addinPath = new DirectoryPath(addinsEnv).MakeAbsolute(_environment);
            }
            else
            {
                addinPath = applicationRoot.Combine("../Addins").Collapse();
            }

            var addinReferences = _processor.InstallAddins(result, addinPath);
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
    }
}
