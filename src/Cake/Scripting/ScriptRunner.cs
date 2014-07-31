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
            IScriptSessionFactory sessionFactory, IScriptAliasGenerator aliasGenerator, 
            IScriptProcessor processor, IScriptHost host)
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

            // Create and prepare the session.
            var session = _sessionFactory.CreateSession(_host);

            // Process the script.
            var context = new ScriptProcessorContext();
            _processor.Process(options.Script, context);

            // Set the working directory.
            _environment.WorkingDirectory = options.Script.MakeAbsolute(_environment).GetDirectory();

            // Load all references.
            var assemblies = new List<Assembly>();
            assemblies.AddRange(GetDefaultAssemblies());
            foreach (var reference in context.References)
            {
                if (_fileSystem.Exist((FilePath)reference))
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

        private static IEnumerable<Assembly> GetDefaultAssemblies()
        {
            var defaultAssemblies = new List<Assembly> {
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
            return defaultAssemblies;
        }

        private static IEnumerable<string> GetDefaultNamespaces()
        {
            var defaultNamespaces = new List<string> {
                "System", "System.Collections.Generic", "System.Linq",
                "System.Text", "System.Threading.Tasks", "System.IO",
                "Cake", "Cake.Core", "Cake.Core.IO", "Cake.Scripting",
                "Cake.Core.Scripting", "Cake.Core.Diagnostics"
            };
            return defaultNamespaces;
        }
    }
}
