using System;
using System.Linq;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.IO.NuGet;

namespace Cake.Core.Scripting.Processors
{
    /// <summary>
    /// Processor for #tool directives.
    /// </summary>
    public sealed class ToolDirectiveProcessor : LineProcessor
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly ICakeLog _log;
        private readonly IToolResolver _nugetToolResolver;
        
        private FilePath _nugetPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolDirectiveProcessor" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="log">The log.</param>
        /// <param name="nugetToolResolver">The NuGet tool resolver.</param>
        public ToolDirectiveProcessor(IFileSystem fileSystem, ICakeEnvironment environment, ICakeLog log, INuGetToolResolver nugetToolResolver)
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

            if (!directive.Equals("#tool", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            // Fetch the tool NuGet ID.
            var toolId = tokens
                .Select(value => value.UnQuote())
                .Skip(1).FirstOrDefault();

            if (string.IsNullOrWhiteSpace(toolId))
            {
                return false;
            }

            // Fetch optional NuGet source.
            var source = tokens
                .Skip(2)
                .Select(value => value.UnQuote())
                .FirstOrDefault();

            // Get the directory path to Cake.
            var applicationRoot = _environment.WorkingDirectory;

            // Get the tool directory.
            var toolsRootDirectoryPath = applicationRoot
                .Combine(".\\tools")
                .Collapse()
                .MakeAbsolute(_environment);

            var toolDirectoryPath = toolsRootDirectoryPath.Combine(toolId);
            var toolsRootDirectory = _fileSystem.GetDirectory(toolsRootDirectoryPath);

            // Create the tool directory if it doesn't exist.
            if (!toolsRootDirectory.Exists)
            {
                _log.Verbose("Creating tool directory {0}", toolsRootDirectoryPath.FullPath);
                toolsRootDirectory.Create();
            }

            // Fetch available tool executables.
            var toolExecutables = GetToolExecutables(toolDirectoryPath);

            // If no executables were found, try install tool from NuGet.
            if (toolExecutables.Length == 0)
            {
                InstallTool(toolId, toolsRootDirectory, source);
                toolExecutables = GetToolExecutables(toolDirectoryPath);
            }

            // Validate found assemblies.
            if (toolExecutables.Length == 0)
            {
                throw new CakeException("Failed to find tool executables.");
            }
            
            _log.Debug(logAction =>
                {
                    foreach (var toolExecutable in toolExecutables)
                    {
                        logAction("Found tool executable: {0}.", toolExecutable.Path);
                    }
                });

            return true;
        }

        private void InstallTool(string toolId, IDirectory toolRootDirectory, string source)
        {
            var nugetPath = GetNuGetPath();
            var runner = new ProcessRunner(_environment, _log);
            var process = runner.Start(nugetPath, new ProcessSettings 
            {
                Arguments = GetNuGetToolInstallArguments(toolId, toolRootDirectory, source)
            });
            process.WaitForExit();
        }

        private FilePath GetNuGetPath()
        {
            var nugetPath = _nugetPath ?? (_nugetPath = _nugetToolResolver.ResolveToolPath());
            if (nugetPath == null)
            {
                throw new CakeException("Failed to find NuGet.");
            }
            return nugetPath;
        }

        private static ProcessArgumentBuilder GetNuGetToolInstallArguments(string toolId, IDirectory toolRootDirectory,
            string source)
        {
            var arguments = new ProcessArgumentBuilder();
            arguments.Append("install");
            arguments.AppendQuoted(toolId);
            arguments.Append("-OutputDirectory");
            arguments.AppendQuoted(toolRootDirectory.Path.FullPath);
            if (!string.IsNullOrWhiteSpace(source))
            {
                arguments.Append("-Source");
                arguments.AppendQuoted(source);
            }
            arguments.Append("-ExcludeVersion -NonInteractive -NoCache");
            return arguments;
        }

        private IFile[] GetToolExecutables(DirectoryPath toolDirectoryPath)
        {
            var toolDirectory = _fileSystem.GetDirectory(toolDirectoryPath);
            return toolDirectory.Exists
                ? toolDirectory.GetFiles("*.exe", SearchScope.Recursive)
                    .ToArray()
                : new IFile[0];
        }
    }
}