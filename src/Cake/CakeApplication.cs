using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using Cake.Bootstrapping;
using Cake.Common.IO;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Scripting;
using Cake.Diagnostics;
using Cake.Scripting;

namespace Cake
{
    public sealed class CakeApplication
    {        
        private readonly ICakeBootstrapper _bootstrapper;
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly CakeLogAdapter _log;
        private readonly IScriptRunner _scriptRunner;

        public CakeApplication(ICakeBootstrapper bootstrapper, IFileSystem fileSystem,
            ICakeEnvironment environment, ICakeLog log, IScriptRunner scriptRunner)
        {
            if (bootstrapper == null)
            {
                throw new ArgumentNullException("bootstrapper");
            }
            if (fileSystem == null)
            {
                throw new ArgumentNullException("fileSystem");
            }
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }
            if (log == null)
            {
                throw new ArgumentNullException("log");
            }
            if (scriptRunner == null)
            {
                throw new ArgumentNullException("scriptRunner");
            }
            _bootstrapper = bootstrapper;
            _fileSystem = fileSystem;
            _log = new CakeLogAdapter(log);
            _environment = environment;
            _scriptRunner = scriptRunner;
        }

        public void Run(CakeOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }
            if (options.Script == null)
            {
                throw new CakeException("No script provided.");
            }

            // Set the log verbosity.
            _log.Verbosity = options.Verbosity;

            // Bootstrap the application.
            _bootstrapper.Bootstrap(_environment.GetApplicationRoot());
            
            // Read the file content.
            var code = ReadSource(options.Script);

            // Update the working directory.
            _environment.WorkingDirectory = GetAbsoluteScriptDirectory(options.Script);

            // Add all references.
            var references = new List<Assembly>
            {
                typeof(Action).Assembly, // mscorlib
                typeof(Uri).Assembly, // System
                typeof(System.Linq.IQueryable).Assembly, // System.Core
                typeof(System.Data.DataTable).Assembly, // System.Data
                typeof(System.Xml.XmlReader).Assembly, // System.Xml
                typeof(System.Xml.Linq.XDocument).Assembly, // System.Xml.Linq
                typeof(Program).Assembly, // Cake
                typeof(ICakeContext).Assembly,  // Cake.Core
                typeof(DirectoryExtensions).Assembly, // Cake.Common
            };

            // Add all namespaces.
            var namespaces = new List<string>
            {
                "System", "System.Collections.Generic", "System.Linq",
                "System.Text", "System.Threading.Tasks", "System.IO",
                "Cake", "Cake.Core", "Cake.Core.IO",  "Cake.Scripting", "Cake.Core.Scripting",
                "Cake.Common", "Cake.Common.IO", 
                "Cake.Common.IO", "Cake.Core.Diagnostics", 
                "Cake.Common.Tools.MSBuild", "Cake.Common.Tools.XUnit", 
                "Cake.Common.Tools.NuGet", "Cake.Common.Tools.NUnit",
                "Cake.Common.Tools.ILMerge"
            };

            // Execute the script.
            var scriptHost = CreateScriptHost(options);
            _scriptRunner.Run(scriptHost, references, namespaces, code);
            if (scriptHost is DescriptionScriptHost)
            {
                Console.WriteLine("{0,-30}{1}","Task", "Description");
                Console.WriteLine(String.Concat(Enumerable.Range(0,79).Select(s => "=")));
                var descriptionScriptHost = (scriptHost as DescriptionScriptHost);
                foreach (var key in descriptionScriptHost.TasksWithDescription.Keys.OrderByDescending(s => s))
                {
                    Console.WriteLine("{0,-30}{1}", key,  descriptionScriptHost.TasksWithDescription[key]);
                }
            }
        }

        private string ReadSource(FilePath path)
        {
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

        private IScriptHost CreateScriptHost(CakeOptions options)
        {
            if (options.ShowDescription)
            {
                return new DescriptionScriptHost(new CakeEngine(
                    _fileSystem, _environment, _log,
                    new CakeArguments(options.Arguments),
                    new Globber(_fileSystem, _environment),
                    new ProcessRunner(_log)));
            }
            return new ScriptHost(new CakeEngine(
                _fileSystem, _environment, _log,
                new CakeArguments(options.Arguments),
                new Globber(_fileSystem, _environment), 
                new ProcessRunner(_log)));
        }
    }
}
