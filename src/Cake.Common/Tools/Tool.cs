using System;
using System.Diagnostics;
using System.Globalization;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Tools
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
        protected void Run(T settings, ToolArgumentBuilder arguments)
        {
            Run(settings, arguments, null);
        }

        /// <summary>
        /// Runs the tool using a custom tool path and the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="arguments">The arguments.</param>
        /// <param name="toolPath">The tool path to use.</param> 
        protected void Run(T settings, ToolArgumentBuilder arguments, FilePath toolPath)
        {
            if (arguments == null)
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
            var info = new ProcessStartInfo(toolPath.FullPath)
            {
                WorkingDirectory = workingDirectory.MakeAbsolute(_environment).FullPath,
                Arguments = arguments.Render(),
                UseShellExecute = false
            };

            // Run the process.
            var process = _processRunner.Start(info);
            if (process == null)
            {
                const string message = "{0}: Process was not started.";
                throw new CakeException(string.Format(CultureInfo.InvariantCulture, message, toolName));
            }

            // Wait for the process to exit.
            process.WaitForExit();

            // Did an error occur?
            if (process.GetExitCode() != 0)
            {
                const string message = "{0}: Process returned an error.";
                throw new CakeException(string.Format(CultureInfo.InvariantCulture, message, toolName));
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
