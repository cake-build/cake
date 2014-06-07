using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Cake.Bootstrapping;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Diagnostics;
using Cake.Scripting;
using Cake.Scripting.Host;

namespace Cake
{
    public sealed class CakeApplication
    {        
        private readonly ICakeBootstrapper _bootstrapper;
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly CakeLogAdapter _log;
        private readonly IScriptRunner _runner;

        public CakeApplication(ICakeBootstrapper bootstrapper = null, IFileSystem fileSystem = null,
            ICakeEnvironment environment = null, ICakeLog log = null, IScriptRunner runner = null)
        {
            _fileSystem = fileSystem ?? new FileSystem();
            _log = new CakeLogAdapter(log ?? new ColoredConsoleBuildLog());
            _bootstrapper = bootstrapper ?? new CakeBootstrapper(_fileSystem, _log);
            _environment = environment ?? new CakeEnvironment();
            _runner = runner ?? new ScriptRunner(_log);
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
                typeof(MSBuild.MSBuildRunner).Assembly, // Cake.MSBuild
                typeof(XUnit.XUnitRunner).Assembly, // Cake.XUnit
                typeof(IO.DirectoryExtensions).Assembly // Cake.IO
            };

            // Add all namespaces.
            var namespaces = new List<string>
            {
                "System", "System.Collections.Generic", "System.Linq",
                "System.Text", "System.Threading.Tasks", "System.IO",
                "Cake", "Cake.Core", "Cake.Core.IO", "Cake.IO",
                "Cake.Core.Diagnostics", "Cake.MSBuild", "Cake.XUnit"
            };

            // Execute the script.
            _runner.Run(CreateScriptHost(options), references, namespaces, code);
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
                    .GetFilePath(scriptPath).GetDirectory();
            }
            return scriptLocation;
        }

        private ScriptHost CreateScriptHost(CakeOptions options)
        {
            return new ScriptHost(new CakeEngine(
                _fileSystem, _environment, _log,
                new CakeArguments(options.Arguments)));
        }
    }
}
