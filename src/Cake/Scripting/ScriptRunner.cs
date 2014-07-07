using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly IScriptHost _host;

        // Delegate factory used by Autofac.
        public delegate ScriptRunner Factory(IScriptHost host);

        public ScriptRunner(IFileSystem fileSystem, ICakeEnvironment environment, CakeArguments arguments,
            IScriptSessionFactory sessionFactory, IScriptAliasGenerator aliasGenerator, IScriptHost host)
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
            _host = host;
        }

        public void Run(CakeOptions options)
        {
            // Initialize the script session factory.
            _sessionFactory.Initialize();

            // Copy the arguments from the options.
            _arguments.SetArguments(options.Arguments);

            // Read the source.
            var code = ReadSource(options.Script);

            // Add all references.            
            var references = GetReferencedAssemblies();
            var namespaces = GetNamespaces();

            // Update the working directory.
            _environment.WorkingDirectory = GetAbsoluteScriptDirectory(options.Script);

            // Run script.
            var session = _sessionFactory.CreateSession(_host);

            // Add references to session.
            foreach (var reference in references)
            {
                session.AddReference(reference);
            }

            // Add namespaces to session.
            foreach (var @namespace in namespaces)
            {
                session.ImportNamespace(@namespace);
            }

            // Find all extension methods and generate proxy methods.
            _aliasGenerator.Generate(session, references);

            // Execute the code.
            session.Execute(code);
        }

        private static IEnumerable<string> GetNamespaces()
        {
            var namespaces = new List<string>
            {
                "System", "System.Collections.Generic", "System.Linq",
                "System.Text", "System.Threading.Tasks", "System.IO",
                "Cake", "Cake.Core", "Cake.Core.IO", "Cake.Scripting", 
                "Cake.Core.Scripting", "Cake.Common", "Cake.Common.IO",
                "Cake.Core.Diagnostics", "Cake.Common.Tools.MSBuild",
                "Cake.Common.Tools.XUnit", "Cake.Common.Tools.NuGet",
                "Cake.Common.Tools.NUnit", "Cake.Common.Tools.ILMerge"
            };
            return namespaces;
        }

        private static List<Assembly> GetReferencedAssemblies()
        {
            var references = new List<Assembly>
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
            return references;
        }

        private string ReadSource(FilePath path)
        {
            path = path.MakeAbsolute(_environment);

            // Get the file and make sure it exist.
            var file = _fileSystem.GetFile(path);
            if (!file.Exists)
            {
                var message = string.Format("Could not find script '{0}'.", path);
                throw new CakeException(message);
            }

            // Read the content from the file.
            using (var stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        private DirectoryPath GetAbsoluteScriptDirectory(FilePath scriptPath)
        {
            // Get the script location.
            var scriptLocation = scriptPath.GetDirectory();
            if (scriptLocation.IsRelative)
            {
                // Concatinate the starting working directory
                // with the script file path.
                scriptLocation = _environment.WorkingDirectory
                    .CombineWithFilePath(scriptPath).GetDirectory();
            }
            return scriptLocation;
        }
    }
}
