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

            // Read the source.
            var processorResult = _processor.Process(options.Script);

            // Set the working directory.
            _environment.WorkingDirectory = processorResult.Root;

            // Run script.
            var session = _sessionFactory.CreateSession(_host);

            // Add references to session.
            var references = LoadReferencedAssemblies(session, processorResult);

            // Add namespaces to session.
            ImportNamespaces(session);

            // Find all extension methods and generate proxy methods.
            _aliasGenerator.Generate(session, references);

            // Execute the code.
            session.Execute(processorResult.Code);
        }

        private IEnumerable<Assembly> LoadReferencedAssemblies(IScriptSession session, ScriptProcessorResult processorResult)
        {
            // Get all default assemblies.
            var assemblies = GetDefaultAssemblies();

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
                    // This reference
                    session.AddReferencePath(reference);
                }
            }

            // Add all loaded assemblies to the session.
            foreach (var reference in assemblies)
            {
                session.AddReference(reference);
            }

            // Return the list of assemblies.
            return assemblies;
        }

        private static List<Assembly> GetDefaultAssemblies()
        {
            return new List<Assembly>
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
        }

        private static void ImportNamespaces(IScriptSession session)
        {
            foreach (var @namespace in GetDefaultNamespaces())
            {
                session.ImportNamespace(@namespace);
            }
        }

        private static IEnumerable<string> GetDefaultNamespaces()
        {
            return new List<string>
            {
                "System", "System.Collections.Generic", "System.Linq",
                "System.Text", "System.Threading.Tasks", "System.IO",
                "Cake", "Cake.Core", "Cake.Core.IO", "Cake.Scripting", 
                "Cake.Core.Scripting", "Cake.Common", "Cake.Common.IO",
                "Cake.Core.Diagnostics", "Cake.Common.Tools.MSBuild",
                "Cake.Common.Tools.XUnit", "Cake.Common.Tools.NuGet",
                "Cake.Common.Tools.NUnit", "Cake.Common.Tools.ILMerge",
                "Cake.Common.Tools.WiX"
            };
        }
    }
}
