using System;
using System.Globalization;
using Cake.Core.IO;

namespace Cake.Core.Utilities
{
    /// <summary>
    /// Base class for tools.
    /// </summary>
    public abstract class Tool<T>
    {
        private readonly IFileSystem _fileSystem;        
        private readonly ICakeEnvironment _environment;
        private readonly IProcessRunner _processRunner;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tool{T}" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        protected Tool(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner)
        {
            _fileSystem = fileSystem;            
            _environment = environment;
            _processRunner = processRunner;
        }

        /// <summary>
        /// Runs the tool using the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="arguments">The arguments.</param>
        protected void Run(T settings, ProcessArgumentBuilder arguments)
        {
            Run(settings, arguments, null);
        }

        /// <summary>
        /// Runs the tool using a custom tool path and the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="arguments">The arguments.</param>
        /// <param name="toolPath">The tool path to use.</param> 
        protected void Run(T settings, ProcessArgumentBuilder arguments, FilePath toolPath)
        {
            Run(settings, arguments, toolPath, null, null);
        }

        /// <summary>
        /// Runs the tool using a custom tool path and the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="arguments">The arguments.</param>
        /// <param name="toolPath">The tool path to use.</param>
        /// <param name="processSettings">The process settings</param>
        /// <param name="postAction">If specified called after process exit</param> 
        protected void Run(T settings, ProcessArgumentBuilder arguments, FilePath toolPath, ProcessSettings processSettings, Action<IProcess> postAction)
        {
            if (arguments == null && (processSettings == null || processSettings.Arguments == null))
            {
                throw new ArgumentNullException("arguments");
            }

            // Get the tool name.
            var toolName = GetToolName();

            // Get the tool path.
            toolPath = GetToolPath(settings, toolPath);
            if (toolPath == null || !_fileSystem.Exist(toolPath))
            {
                const string message = "{0}: Could not locate executable.";
                throw new CakeException(string.Format(CultureInfo.InvariantCulture, message, toolName));
            }

            // Get the working directory.
            var workingDirectory = GetWorkingDirectory(settings);
            if (workingDirectory == null)
            {
                const string message = "{0}: Could not resolve working directory.";
                throw new CakeException(string.Format(CultureInfo.InvariantCulture, message, toolName));                
            }

            // Create the process start info.
            var info = processSettings ?? new ProcessSettings();
            if (info.Arguments == null)
            {
                info.Arguments = arguments;
            }
            if (info.WorkingDirectory == null)
            {
                info.WorkingDirectory = workingDirectory.MakeAbsolute(_environment).FullPath;
            }

            // Run the process.
            var process = _processRunner.Start(toolPath, info);
            if (process == null)
            {
                const string message = "{0}: Process was not started.";
                throw new CakeException(string.Format(CultureInfo.InvariantCulture, message, toolName));
            }

            // Wait for the process to exit.
            process.WaitForExit();

            try
            {
                // Did an error occur?
                if (process.GetExitCode() != 0)
                {
                    const string message = "{0}: Process returned an error.";
                    throw new CakeException(string.Format(CultureInfo.InvariantCulture, message, toolName));
                }
            }
            finally
            {
                // Post action specified?
                if (postAction != null)
                {
                    postAction(process);
                }
            }
        }

        /// <summary>
        /// Gets the tool path.
        /// </summary>
        /// <returns>The tool path.</returns>
        protected FilePath GetToolPath(T settings, FilePath toolPath)
        {
            if (toolPath != null)
            {
                return toolPath.MakeAbsolute(_environment);
            }
            var defaultToolPath = GetDefaultToolPath(settings);
            return defaultToolPath != null
                ? defaultToolPath.MakeAbsolute(_environment)
                : null;
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected abstract string GetToolName();

        /// <summary>
        /// Gets the working directory.
        /// Defaults to the currently set working directory.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The working directory for the tool.</returns>
        protected virtual DirectoryPath GetWorkingDirectory(T settings)
        {
            return _environment.WorkingDirectory;
        }

        /// <summary>
        /// Gets the default tool path.
        /// </summary>
        /// <returns>The default tool path.</returns>
        protected abstract FilePath GetDefaultToolPath(T settings);
    }
}
