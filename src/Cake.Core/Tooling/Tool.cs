// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        private readonly IToolLocator _tools;
        private readonly IProcessRunner _processRunner;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tool{TSettings}"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        protected Tool(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException(nameof(fileSystem));
            }
            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }
            if (processRunner == null)
            {
                throw new ArgumentNullException(nameof(processRunner));
            }

            _fileSystem = fileSystem;
            _environment = environment;
            _processRunner = processRunner;
            _tools = tools;
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
        /// <param name="processSettings">The process settings.</param>
        /// <param name="postAction">If specified called after process exit.</param>
        protected void Run(
            TSettings settings,
            ProcessArgumentBuilder arguments,
            ProcessSettings processSettings,
            Action<IProcess> postAction)
        {
            if (arguments == null && (processSettings?.Arguments == null))
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            var process = RunProcess(settings, arguments, processSettings);

            // Wait for the process to exit.
            if (settings.ToolTimeout.HasValue)
            {
                if (!process.WaitForExit((int)settings.ToolTimeout.Value.TotalMilliseconds))
                {
                    const string message = "Tool timeout ({0}): {1}";
                    throw new TimeoutException(string.Format(CultureInfo.InvariantCulture, message, settings.ToolTimeout.Value, GetToolName()));
                }
            }
            else
            {
                process.WaitForExit();
            }

            // Post action specified?
            postAction?.Invoke(process);

            var exitCode = process.GetExitCode();
            if (!settings.HandleExitCode?.Invoke(exitCode) ?? true)
            {
                ProcessExitCode(process.GetExitCode());
            }
        }

        /// <summary>
        /// Customized exit code handling.
        /// Standard behavior is to fail when non zero.
        /// </summary>
        /// <param name="exitCode">The process exit code.</param>
        protected virtual void ProcessExitCode(int exitCode)
        {
            // Did an error occur?
            if (exitCode != 0)
            {
                const string message = "{0}: Process returned an error (exit code {1}).";
                throw new CakeException(string.Format(CultureInfo.InvariantCulture, message, GetToolName(), exitCode));
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
        /// <param name="processSettings">The process settings.</param>
        /// <returns>The process that the tool is running under.</returns>
        protected IProcess RunProcess(
            TSettings settings,
            ProcessArgumentBuilder arguments,
            ProcessSettings processSettings)
        {
            if (arguments == null && (processSettings?.Arguments == null))
            {
                throw new ArgumentNullException(nameof(arguments));
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
            if (info.EnvironmentVariables == null)
            {
                info.EnvironmentVariables = GetEnvironmentVariables(settings);
            }

            // Want to opt out of using a working directory?
            info.NoWorkingDirectory = settings.NoWorkingDirectory;

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
        /// <param name="settings">The settings.</param>
        /// <returns>The name of the tool.</returns>
        protected virtual IEnumerable<string> GetToolExecutableNames(TSettings settings) => GetToolExecutableNames();

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
            if (settings.WorkingDirectory != null)
            {
                return settings.WorkingDirectory;
            }

            return _environment.WorkingDirectory;
        }

        /// <summary>
        /// Gets alternative file paths which the tool may exist in.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The default tool path.</returns>
        protected virtual IEnumerable<FilePath> GetAlternativeToolPaths(TSettings settings)
        {
            return Enumerable.Empty<FilePath>();
        }

        /// <summary>
        /// Gets the environment variables.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The environment variables for the tool.</returns>
        protected virtual IDictionary<string, string> GetEnvironmentVariables(TSettings settings)
        {
            return settings.EnvironmentVariables;
        }

        /// <summary>
        /// Gets the resolved tool path.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The resolved tool path.</returns>
        protected FilePath GetToolPath(TSettings settings)
        {
            return GetToolPathUsingToolService(settings);
        }

        private FilePath GetToolPathUsingToolService(TSettings settings)
        {
            var toolPath = settings.ToolPath;
            if (toolPath != null)
            {
                return toolPath.MakeAbsolute(_environment);
            }

            // Look for each possible executable name in various places.
            var result = _tools.Resolve(GetToolExecutableNames(settings));
            if (result != null)
            {
                return result;
            }

            // Look through all the alternative directories for the tool.
            var alternativePaths = GetAlternativeToolPaths(settings) ?? Enumerable.Empty<FilePath>();
            foreach (var alternativePath in alternativePaths)
            {
                if (_fileSystem.Exist(alternativePath))
                {
                    return alternativePath.MakeAbsolute(_environment);
                }
            }

            return null;
        }
    }
}