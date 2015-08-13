using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.IO.NuGet;

namespace Cake.Core.Scripting.Processors
{
    /// <summary>
    /// Processor for #addin directives.
    /// </summary>
    public sealed class AddInDirectiveProcessor : LineProcessor
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly ICakeLog _log;
        private readonly IToolResolver _nugetToolResolver;

        private FilePath _nugetPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddInDirectiveProcessor" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="log">The log.</param>
        /// <param name="nugetToolResolver">The NuGet tool resolver.</param>
        public AddInDirectiveProcessor(IFileSystem fileSystem, ICakeEnvironment environment, ICakeLog log, INuGetToolResolver nugetToolResolver)
            : base(environment)
        {
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
            if (nugetToolResolver == null)
            {
                throw new ArgumentNullException("nugetToolResolver");
            }

            _fileSystem = fileSystem;
            _environment = environment;
            _log = log;
            _nugetToolResolver = nugetToolResolver;
        }

        /// <summary>
        /// Processes the specified line.
        /// </summary>
        /// <param name="processor">The script processor.</param>
        /// <param name="context">The script processor context.</param>
        /// <param name="currentScriptPath">The current script path.</param>
        /// <param name="line">The line to process.</param>
        /// <returns>
        ///   <c>true</c> if the processor handled the line; otherwise <c>false</c>.
        /// </returns>
        public override bool Process(IScriptProcessor processor, ScriptProcessorContext context, FilePath currentScriptPath, string line)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var tokens = Split(line);
            var directive = tokens.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(directive))
            {
                return false;
            }

            if (!directive.Equals("#addin", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            // Fetch the addin NuGet ID.
            var addInId = tokens
                .Select(value => value.UnQuote())
                .Skip(1).FirstOrDefault();

            if (string.IsNullOrWhiteSpace(addInId))
            {
                return false;
            }

            // Fetch optional NuGet source.
            var source = tokens
                .Skip(2)
                .Select(value => value.UnQuote())
                .FirstOrDefault();

            // Get the directory path to Cake.
            var applicationRoot = _environment.GetApplicationRoot();

            // Get the addin directory.
            var addInRootDirectoryPath = applicationRoot
                .Combine("..\\Addins")
                .Collapse()
                .MakeAbsolute(_environment);

            var addInDirectoryPath = addInRootDirectoryPath.Combine(addInId);
            var addInRootDirectory = _fileSystem.GetDirectory(addInRootDirectoryPath);

            // Create the addin directory if it doesn't exist.
            if (!addInRootDirectory.Exists)
            {
                _log.Verbose("Creating addin directory {0}", addInRootDirectoryPath.FullPath);
                addInRootDirectory.Create();
            }

            // Fetch available addin assemblies.
            var addInAssemblies = GetAddInAssemblies(addInRootDirectory, addInDirectoryPath);

            // If no assemblies were found, try install addin from NuGet.
            if (addInAssemblies.Count == 0)
            {
                InstallAddin(addInId, addInRootDirectory, source);
                addInAssemblies = GetAddInAssemblies(addInRootDirectory, addInDirectoryPath);
            }

            // Validate found assemblies.
            if (addInAssemblies.Count == 0)
            {
                throw new CakeException("Failed to find AddIn assemblies");
            }

            // Reference found assemblies.
            foreach (var assemblyPath in addInAssemblies.Select(assembly => assembly.Path.FullPath))
            {
                _log.Verbose("Addin: {0}, adding Reference {1}", addInId, assemblyPath);
                context.AddReference(assemblyPath);
            }

            return true;
        }

        private void InstallAddin(string addInId, IDirectory addInRootDirectory, string source)
        {
            var nugetPath = GetNuGetPath();
            var runner = new ProcessRunner(_environment, _log);
            var process = runner.Start(nugetPath, new ProcessSettings 
            {
                Arguments = GetNuGetAddinInstallArguments(addInId, addInRootDirectory, source)
            });
            process.WaitForExit();
        }

        private FilePath GetNuGetPath()
        {
            var nugetPath = _nugetPath ?? (_nugetPath = _nugetToolResolver.ResolveToolPath());
            if (nugetPath == null)
            {
                throw new CakeException("Failed to find NuGet");
            }
            return nugetPath;
        }

        private static ProcessArgumentBuilder GetNuGetAddinInstallArguments(string addInId, IDirectory addInRootDirectory, string source)
        {
            var arguments = new ProcessArgumentBuilder();
            arguments.Append("install");
            arguments.AppendQuoted(addInId);
            arguments.Append("-OutputDirectory");
            arguments.AppendQuoted(addInRootDirectory.Path.FullPath);
            if (!string.IsNullOrWhiteSpace(source))
            {
                arguments.Append("-Source");
                arguments.AppendQuoted(source);
            }
            arguments.Append("-ExcludeVersion -NonInteractive -NoCache");
            return arguments;
        }

        private IList<IFile> GetAddInAssemblies(IDirectory addInRootDirectory, DirectoryPath addInDirectoryPath)
        {
            IList<DirectoryPath> packages = GetAddInPackages(addInRootDirectory, addInDirectoryPath);
            packages.Add(addInDirectoryPath);

            List<IFile> files = new List<IFile>();
            foreach (DirectoryPath package in packages)
            {
                files.AddRange(GetAddInAssemblies(package));
            }

            return files;
        }

        private IList<IFile> GetAddInAssemblies(DirectoryPath addInDirectoryPath)
        {
            var addInDirectory = _fileSystem.GetDirectory(addInDirectoryPath);
            return addInDirectory.Exists
                ? addInDirectory.GetFiles("*.dll", SearchScope.Recursive)
                    .Where(file => !file.Path.FullPath.EndsWith("Cake.Core.dll", StringComparison.OrdinalIgnoreCase))
                    .ToList()
                : new List<IFile>();
        }

        private IList<DirectoryPath> GetAddInPackages(IDirectory addInRootDirectory, DirectoryPath addInDirectoryPath)
        {
            IList<DirectoryPath> dirs = new List<DirectoryPath>();

            var addInDirectory = _fileSystem.GetDirectory(addInDirectoryPath);
            var path = new FilePath(addInDirectoryPath.FullPath + "/packages.config");

            var file = _fileSystem.GetFile(path);

            if (file.Exists)
            {
                using (var fileStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                using (var xmlReader = XmlReader.Create(fileStream))
                {
                    var document = new XmlDocument();
                    document.Load(xmlReader);

                    var nodes = document.SelectNodes("/packages/package");

                    if (nodes == null || nodes.Count == 0)
                    {
                        throw new CakeException(string.Format("Failed to find package nodes in '{0}'", path.FullPath));
                    }

                    foreach (XmlNode node in nodes)
                    {
                        try
                        {
                            dirs.Add(new DirectoryPath(addInRootDirectory.Path + "/" + node.Attributes["id"].Value));
                        }
                        catch
                        {
                            throw new CakeException(string.Format("Package missing id attribute."));
                        }
                    }
                }
            }

            return dirs;
        }
    }
}