using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cake.Core.IO;

namespace Cake.Core.Tooling
{
    /// <summary>
    /// Base class for tools.
    /// </summary>
    /// <typeparam name="TSettings">The settings type.</typeparam>
    public abstract class Tool<TSettings> where TSettings : ToolSettings
    {
        private readonly ICakeEnvironment _environment;
        private readonly IFileSystem _fileSystem;
        private readonly IGlobber _globber;
        private readonly IProcessRunner _processRunner;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tool{TSettings}" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="globber">The globber.</param>
        protected Tool(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner,
            IGlobber globber)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException("fileSystem");
            }
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }
            if (processRunner == null)
            {
                throw new ArgumentNullException("processRunner");
            }
            if (globber == null)
            {
                throw new ArgumentNullException("globber");
            }

            _fileSystem = fileSystem;
            _environment = environment;
            _processRunner = processRunner;
            _globber = globber;
        }

        /// <summary>
        /// Runs the tool using the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="arguments">The arguments.</param>
        protected void Run(TSettings settings, ProcessArgumentBuilder arguments)
        {
            Run(settings, arguments, null, null);
        }

        /// <summary>
        /// Runs the tool using a custom tool path and the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="arguments">The arguments.</param>
        /// <param name="processSettings">The process settings</param>
        /// <param name="postAction">If specified called after process exit</param>
        protected void Run(
            TSettings settings,
            ProcessArgumentBuilder arguments,
            ProcessSettings processSettings,
            Action<IProcess> postAction)
        {
            if (arguments == null && (processSettings == null || processSettings.Arguments == null))
            {
                throw new ArgumentNullException("arguments");
            }

            var process = RunProcess(settings, arguments, processSettings);

            // Wait for the process to exit.
            process.WaitForExit();

            try
            {
                // Did an error occur?
                if (process.GetExitCode() != 0)
                {
                    const string message = "{0}: Process returned an error (exit code {1}).";
                    throw new CakeException(string.Format(CultureInfo.InvariantCulture, message, GetToolName(), process.GetExitCode()));
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
        /// Runs the tool using a custom tool path and the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns>The process that the tool is running under.</returns>
        protected IProcess RunProcess(TSettings settings, ProcessArgumentBuilder arguments)
        {
            return RunProcess(settings, arguments, null);
        }

        /// <summary>
        /// Runs the tool using a custom tool path and the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="arguments">The arguments.</param>
        /// <param name="processSettings">The process settings</param>
        /// <returns>The process that the tool is running under.</returns>
        protected IProcess RunProcess(
            TSettings settings,
            ProcessArgumentBuilder arguments,
            ProcessSettings processSettings)
        {
            if (arguments == null && (processSettings == null || processSettings.Arguments == null))
            {
                throw new ArgumentNullException("arguments");
            }

            // Should we customize the arguments?
            if (settings.ArgumentCustomization != null)
            {
                arguments = settings.ArgumentCustomization(arguments);
            }

            // Get the tool name.
            var toolName = GetToolName();

            // Get the tool path.
            var toolPath = GetToolPath(settings);
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
            return process;
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected abstract string GetToolName();

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected abstract IEnumerable<string> GetToolExecutableNames();

        /// <summary>
        /// Gets the working directory.
        /// Defaults to the currently set working directory.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The working directory for the tool.</returns>
        protected virtual DirectoryPath GetWorkingDirectory(TSettings settings)
        {
            return _environment.WorkingDirectory;
        }

        /// <summary>
        /// Gets alternative file paths which the tool may exist in
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The default tool path.</returns>
        protected virtual IEnumerable<FilePath> GetAlternativeToolPaths(TSettings settings)
        {
            return Enumerable.Empty<FilePath>();
        }

        /// <summary>
        /// Gets the tool path
        /// </summary>
        /// <param name="settings">The settings</param>
        /// <returns>The full path to the tool</returns>
        protected virtual FilePath GetToolPath(TSettings settings)
        {
            var alternativePaths = GetAlternativeToolPaths(settings) ?? Enumerable.Empty<FilePath>();
            var toolExeNames = GetToolExecutableNames();

            return this.GetToolPath(settings.ToolPath, alternativePaths, toolExeNames);
        }

        /// <summary>
        /// Gets the tool path
        /// </summary>
        /// <param name="toolPath">The tool path to be used</param>
        /// <param name="alternativePaths">The alternatives paths to be used to look for the tools</param>
        /// <param name="toolExeNames">The exe names to be looked for</param>
        /// <returns>The full path to the tool</returns>
        protected virtual FilePath GetToolPath(FilePath toolPath, IEnumerable<FilePath> alternativePaths, IEnumerable<string> toolExeNames)
        {
            if (toolPath != null)
            {
                return toolPath.MakeAbsolute(_environment);
            }

            IEnumerable<string> pathDirs = null;

            // Look for each possible executable name in various places.
            foreach (var toolExeName in toolExeNames)
            {
                // First look in ./tools/
                toolPath = _globber.GetFiles("./tools/**/" + toolExeName).FirstOrDefault();
                if (toolPath != null)
                {
                    return toolPath.MakeAbsolute(_environment);
                }

                // Cache the PATH directory list if we didn't already.
                if (pathDirs == null)
                {
                    var pathEnv = _environment.GetEnvironmentVariable("PATH");
                    if (!string.IsNullOrEmpty(pathEnv))
                    {
                        pathDirs = pathEnv.Split(new char[] { _environment.IsUnix() ? ':' : ';' }, StringSplitOptions.RemoveEmptyEntries);
                    }
                    else
                    {
                        pathDirs = Enumerable.Empty<string>();
                    }
                }

                // Look in every PATH directory for the file.
                foreach (var pathDir in pathDirs)
                {
                    var file = new DirectoryPath(pathDir).CombineWithFilePath(toolExeName);

                    if (_fileSystem.Exist(file))
                    {
                        return file.MakeAbsolute(_environment);
                    }
                }
            }

            // Look through all the alternative directories for the tool.
            foreach (var altPath in alternativePaths)
            {
                if (_fileSystem.Exist(altPath))
                {
                    return altPath.MakeAbsolute(_environment);
                }
            }

            return null;
        }
    }
}