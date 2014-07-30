using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cake.Common.IO;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Scripting;

namespace Cake.Scripting
{
    internal sealed class ScriptRunner
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly CakeArguments _arguments;
        private readonly IScriptSessionFactory _sessionFactory;
        private readonly IScriptAliasGenerator _aliasGenerator;
        private readonly IScriptProcessor _processor;
        private readonly IScriptHost _host;

        // Delegate factory used by Autofac.
        public delegate ScriptRunner Factory(IScriptHost host);

        public ScriptRunner(IFileSystem fileSystem, ICakeEnvironment environment, CakeArguments arguments,
            IScriptSessionFactory sessionFactory, IScriptAliasGenerator aliasGenerator, IScriptProcessor processor, IScriptHost host)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException("fileSystem");
            }
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }
            if (arguments == null)
            {
                throw new ArgumentNullException("arguments");
            }
            if (sessionFactory == null)
            {
                throw new ArgumentNullException("sessionFactory");
            }
            if (aliasGenerator == null)
            {
                throw new ArgumentNullException("aliasGenerator");
            }
            if (processor == null)
            {
                throw new ArgumentNullException("processor");
            }
            if (host == null)
            {
                throw new ArgumentNullException("host");
            }
            _fileSystem = fileSystem;
            _environment = environment;
            _arguments = arguments;
            _sessionFactory = sessionFactory;
            _aliasGenerator = aliasGenerator;
            _processor = processor;
            _host = host;
        }

        public void Run(CakeOptions options)
        {
            // Initialize the script session factory.
            _sessionFactory.Initialize();

            // Copy the arguments from the options.
            _arguments.SetArguments(options.Arguments);

            // Create a new context to keep track of what scripts have been loaded and so on.
            var context = new ScriptRunnerContext(new PathComparer(_environment.IsUnix()));

            // Create and prepare the session.
            var session = _sessionFactory.CreateSession(_host);
            PrepareSession(session);

            // Read the source.
            var result = _processor.Process(options.Script);

            // Set the working directory.
            _environment.WorkingDirectory = result.Root;

            // Process the script.
            ExecuteScript(session, context, result);
        }

        private void PrepareSession(IScriptSession session)
        {
            // Load default assemblies.
            var defaultAssemblies = LoadDefaultAssemblies(session);

            // Add namespaces to session.
            ImportDefaultNamespaces(session);

            // Find all extension methods and generate proxy methods.
            _aliasGenerator.Generate(session, defaultAssemblies);
        }

        private void ExecuteScript(IScriptSession session, ScriptRunnerContext context, ScriptProcessorResult result)
        {
            if (result.Scripts.Count > 0)
            {
                foreach (var scriptPath in result.Scripts)
                {
                    // TODO: Make script path absolute to the current script.
                    var absoluteScriptPath = scriptPath.MakeAbsolute(result.Root);
                    if (!context.Exist(absoluteScriptPath))
                    {
                        var scriptResult = _processor.Process(absoluteScriptPath);
                        context.AddScript(absoluteScriptPath);

                        // Execute the script recursivly.
                        ExecuteScript(session, context, scriptResult);
                    }
                }
            }

            // Add script references to session.
            var references = LoadReferencedAssemblies(session, result);
            if (references.Count > 0)
            {
                // Find all extension methods and generate proxy methods.    
                _aliasGenerator.Generate(session, references);
            }

            session.Execute(result.Code);
        }

        private IList<Assembly> LoadReferencedAssemblies(IScriptSession session, ScriptProcessorResult processorResult)
        {
            // Get all default assemblies.
            var assemblies = new List<Assembly>();

            // Try to load assemblies we find on disc.
            // This way we get script aliases for free.
            foreach (var reference in processorResult.References)
            {
                var absoluteReferencePath = reference.MakeAbsolute(_environment);
                if (_fileSystem.Exist(absoluteReferencePath))
                {
                    var assembly = Assembly.LoadFile(absoluteReferencePath.FullPath);
                    assemblies.Add(assembly);
                }
                else
                {
                    // Add a reference to the session.
                    session.AddReferencePath(reference);
                }
            }

            LoadAssemblies(session, assemblies);
            
            return assemblies;
        }

        private static IEnumerable<Assembly> LoadDefaultAssemblies(IScriptSession session)
        {
            var defaultAssemblies = new List<Assembly>
            {
                typeof (Action).Assembly, // mscorlib
                typeof (Uri).Assembly, // System
                typeof (IQueryable).Assembly, // System.Core
                typeof (System.Data.DataTable).Assembly, // System.Data
                typeof (System.Xml.XmlReader).Assembly, // System.Xml
                typeof (System.Xml.Linq.XDocument).Assembly, // System.Xml.Linq
                typeof (Program).Assembly, // Cake
                typeof (ICakeContext).Assembly, // Cake.Core
                typeof (DirectoryExtensions).Assembly, // Cake.Common
            };
            LoadAssemblies(session, defaultAssemblies);
            return defaultAssemblies;
        }

        private static void ImportDefaultNamespaces(IScriptSession session)
        {
            var defaultNamespaces = new List<string>
            {
                "System", "System.Collections.Generic", "System.Linq",
                "System.Text", "System.Threading.Tasks", "System.IO",
                "Cake", "Cake.Core", "Cake.Core.IO", "Cake.Scripting", 
                "Cake.Core.Scripting", "Cake.Core.Diagnostics"
            };
            foreach (var @namespace in defaultNamespaces)
            {
                session.ImportNamespace(@namespace);
            }
        }

        private static void LoadAssemblies(IScriptSession session, IEnumerable<Assembly> assemblies)
        {
            foreach (var reference in assemblies)
            {
                session.AddReference(reference);
            }
        }
    }
}
