using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cake.Core.IO;

namespace Cake.Core.Scripting
{
    /// <summary>
    /// Responsible for running scripts.
    /// </summary>
    public sealed class ScriptRunner : IScriptRunner
    {
        private readonly IScriptEngine _engine;
        private readonly IScriptAliasFinder _aliasFinder;
        private readonly IScriptProcessor _scriptProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptRunner"/> class.
        /// </summary>
        /// <param name="engine">The session factory.</param>
        /// <param name="aliasFinder">The alias generator.</param>
        /// <param name="scriptProcessor">The script processor.</param>
        public ScriptRunner(IScriptEngine engine, IScriptAliasFinder aliasFinder, IScriptProcessor scriptProcessor)
        {
            if (engine == null)
            {
                throw new ArgumentNullException("engine");
            }
            if (aliasFinder == null)
            {
                throw new ArgumentNullException("aliasFinder");
            }
            _engine = engine;
            _aliasFinder = aliasFinder;
            _scriptProcessor = scriptProcessor;
        }

        /// <summary>
        /// Runs the script using the specified script host.
        /// </summary>
        /// <param name="host">The script host.</param>
        /// <param name="script">The script.</param>
        /// <param name="arguments">The arguments.</param>
        public void Run(IScriptHost host, FilePath script, IDictionary<string, string> arguments)
        {
            if (host == null)
            {
                throw new ArgumentNullException("host");
            }
            if (script == null)
            {
                throw new ArgumentNullException("script");
            }
            if (arguments == null)
            {
                throw new ArgumentNullException("arguments");
            }

            // Copy the arguments from the options.
            host.Context.Arguments.SetArguments(arguments);

            // Set the working directory.
            host.Context.Environment.WorkingDirectory = script.MakeAbsolute(host.Context.Environment).GetDirectory();

            // Process the script.
            var context = new ScriptProcessorContext();
            _scriptProcessor.Process(script.GetFilename(), context);

            // Create and prepare the session.
            var session = _engine.CreateSession(host, arguments);

            // Load all references.
            var assemblies = new List<Assembly>();
            assemblies.AddRange(GetDefaultAssemblies(host.Context.FileSystem));
            foreach (var reference in context.References)
            {
                if (host.Context.FileSystem.Exist((FilePath)reference))
                {
                    var assembly = Assembly.LoadFile(reference);
                    assemblies.Add(assembly);
                }
                else
                {
                    // Add a reference to the session.
                    session.AddReferencePath(reference);
                }
            }

            var aliases = new List<ScriptAlias>();

            // Got any assemblies?
            if (assemblies.Count > 0)
            {
                // Find all script aliases.
                var foundAliases = _aliasFinder.FindAliases(assemblies);
                if (foundAliases.Length > 0)
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
            var namespaces = new HashSet<string>(context.Namespaces, StringComparer.Ordinal);
            namespaces.AddRange(GetDefaultNamespaces());
            namespaces.AddRange(aliases.SelectMany(x => x.Namespaces));
            foreach (var @namespace in namespaces.OrderBy(ns => ns))
            {
                session.ImportNamespace(@namespace);
            }

            // Generate the script code.
            var code = GenerateCode(context, aliases);

            // Execute the script code.
            session.Execute(code);
        }

        private string GenerateCode(ScriptProcessorContext context, IEnumerable<ScriptAlias> aliases)
        {
            var generator = _engine.GetCodeGenerator();
            var script = new Script(context.Namespaces, context.Lines, aliases);
            var code = generator.Generate(script);
            return code;
        }

        private static IEnumerable<Assembly> GetDefaultAssemblies(IFileSystem fileSystem)
        {
            var defaultAssemblies = new HashSet<Assembly> 
            {
                typeof(Action).Assembly, // mscorlib
                typeof(Uri).Assembly, // System
                typeof(IQueryable).Assembly, // System.Core
                typeof(System.Data.DataTable).Assembly, // System.Data
                typeof(System.Xml.XmlReader).Assembly, // System.Xml
                typeof(System.Xml.Linq.XDocument).Assembly, // System.Xml.Linq
                typeof(ICakeContext).Assembly, // Cake.Core
            };

            // Load other assemblies that we need.
            // TODO: Make this less hackish...
            var assemblyPath = new FilePath(typeof(ScriptRunner).Assembly.Location);
            var assemblyDirectory = fileSystem.GetDirectory(assemblyPath.GetDirectory());
            var patterns = new[] { "Cake.Common.dll", "Cake.exe" };
            var loaded = new HashSet<FilePath>();
            foreach (var pattern in patterns)
            {
                var cakeAssemblies = assemblyDirectory.GetFiles(pattern, SearchScope.Current);
                foreach (var cakeAssembly in cakeAssemblies)
                {
                    var assembly = Assembly.LoadFile(cakeAssembly.Path.FullPath);
                    defaultAssemblies.Add(assembly);
                    loaded.Add(pattern);
                }
            }

            return defaultAssemblies;
        }

        private static IEnumerable<string> GetDefaultNamespaces()
        {
            var defaultNamespaces = new HashSet<string> 
            {
                "System", "System.Collections.Generic", "System.Linq",
                "System.Text", "System.Threading.Tasks", "System.IO",
                "Cake.Core", "Cake.Core.IO",
                "Cake.Core.Scripting", "Cake.Core.Diagnostics"
            };
            return defaultNamespaces;
        }
    }
}
