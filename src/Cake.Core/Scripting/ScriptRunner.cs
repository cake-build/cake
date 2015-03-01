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
        private readonly IScriptSessionFactory _sessionFactory;
        private readonly IScriptAliasGenerator _aliasGenerator;
        private readonly IScriptProcessor _scriptProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptRunner"/> class.
        /// </summary>
        /// <param name="sessionFactory">The session factory.</param>
        /// <param name="aliasGenerator">The alias generator.</param>
        /// <param name="scriptProcessor">The script processor.</param>
        public ScriptRunner(IScriptSessionFactory sessionFactory, IScriptAliasGenerator aliasGenerator, IScriptProcessor scriptProcessor)
        {
            if (sessionFactory == null)
            {
                throw new ArgumentNullException("sessionFactory");
            }
            if (aliasGenerator == null)
            {
                throw new ArgumentNullException("aliasGenerator");
            }
            _sessionFactory = sessionFactory;
            _aliasGenerator = aliasGenerator;
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

            // Validate the script host.
            ValidateScriptHost(host);

            // SetArguments the script session factory.
            _sessionFactory.Initialize();

            // Copy the arguments from the options.
            host.Arguments.SetArguments(arguments);

            // Set the working directory.
            host.Environment.WorkingDirectory = script.MakeAbsolute(host.Environment).GetDirectory();

            // Make sure that any directories are stripped from the script path.
            script = script.GetFilename();

            // Create and prepare the session.
            var session = _sessionFactory.CreateSession(host);

            // Process the script.
            var context = new ScriptProcessorContext();
            _scriptProcessor.Process(script, context);

            // Load all references.
            var assemblies = new List<Assembly>();
            assemblies.AddRange(GetDefaultAssemblies(host.FileSystem));
            foreach (var reference in context.References)
            {
                if (host.FileSystem.Exist((FilePath)reference))
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

            // Got any assemblies?
            if (assemblies.Count > 0)
            {
                // Find all extension methods and generate proxy methods.    
                _aliasGenerator.GenerateScriptAliases(context, assemblies);

                // Add assembly references to the session.
                foreach (var assembly in assemblies)
                {
                    session.AddReference(assembly);
                }
            }

            // Import all namespaces.
            var namespaces = new List<string>(context.Namespaces);
            namespaces.AddRange(GetDefaultNamespaces());
            foreach (var @namespace in namespaces.OrderBy(ns => ns))
            {
                session.ImportNamespace(@namespace);
            }

            // Execute the script.
            session.Execute(context.GetScriptCode());
        }

        // ReSharper disable once UnusedParameter.Local
        private static void ValidateScriptHost(IScriptHost host)
        {
            if (host.FileSystem == null)
            {
                throw new ArgumentException("Script host has no file system.");
            }
            if (host.Environment == null)
            {
                throw new ArgumentException("Script host has no environment.");
            }
            if (host.Arguments == null)
            {
                throw new ArgumentException("Script host has no arguments.");
            }
        }

        private IEnumerable<Assembly> GetDefaultAssemblies(IFileSystem fileSystem)
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
