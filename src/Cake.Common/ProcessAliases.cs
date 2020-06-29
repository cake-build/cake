// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common
{
    /// <summary>
    /// Contains functionality related to processes.
    /// </summary>
    [CakeAliasCategory("Process")]
    public static class ProcessAliases
    {
        /// <summary>
        /// Starts the process resource that is specified by the filename.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="fileName">The file name.</param>
        /// <returns>The exit code that the started process specified when it terminated.</returns>
        /// <example>
        /// <code>
        /// var exitCodeWithoutArguments = StartProcess("ping");
        /// // This should output 1 as argument is missing
        /// Information("Exit code: {0}", exitCodeWithoutArguments);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static int StartProcess(this ICakeContext context, FilePath fileName)
        {
            return StartProcess(context, fileName, new ProcessSettings());
        }

        /// <summary>
        /// Starts the process resource that is specified by the filename and arguments.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="processArguments">The arguments used in the process settings.</param>
        /// <returns>The exit code that the started process specified when it terminated.</returns>
        /// <example>
        /// <code>
        /// var exitCodeWithArgument = StartProcess("ping", "localhost");
        /// // This should output 0 as valid arguments supplied
        /// Information("Exit code: {0}", exitCodeWithArgument);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static int StartProcess(this ICakeContext context, FilePath fileName, string processArguments)
        {
            return StartProcess(context, fileName, new ProcessSettings { Arguments = processArguments });
        }

        /// <summary>
        /// Starts the process resource that is specified by the filename and settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>The exit code that the started process specified when it terminated.</returns>
        /// <example>
        /// <code>
        /// var exitCodeWithArgument = StartProcess("ping", new ProcessSettings{ Arguments = "localhost" });
        /// // This should output 0 as valid arguments supplied
        /// Information("Exit code: {0}", exitCodeWithArgument);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static int StartProcess(this ICakeContext context, FilePath fileName, ProcessSettings settings)
        {
            return StartProcess(context, fileName, settings, out var redirectedOutput);
        }

        /// <summary>
        /// Starts the process resource that is specified by the filename and settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="redirectedStandardOutput">Returns process output if <see cref="ProcessSettings.RedirectStandardOutput"/> is true.
        /// Otherwise <c>null</c> is returned.</param>
        /// <returns>The exit code that the started process specified when it terminated.</returns>
        /// <example>
        /// <code>
        /// IEnumerable&lt;string&gt; redirectedStandardOutput;
        /// var exitCodeWithArgument =
        ///     StartProcess(
        ///         "ping",
        ///         new ProcessSettings {
        ///             Arguments = "localhost",
        ///             RedirectStandardOutput = true
        ///         },
        ///         out redirectedStandardOutput
        ///     );
        ///
        /// // Output last line of process output.
        /// Information("Last line of output: {0}", redirectedStandardOutput.LastOrDefault());
        ///
        /// // This should output 0 as valid arguments supplied
        /// Information("Exit code: {0}", exitCodeWithArgument);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static int StartProcess(this ICakeContext context, FilePath fileName, ProcessSettings settings, out IEnumerable<string> redirectedStandardOutput)
        {
            var process = StartAndReturnProcess(context, fileName, settings);

            // Wait for the process to stop.
            if (settings.Timeout.HasValue)
            {
                if (!process.WaitForExit(settings.Timeout.Value))
                {
                    throw new TimeoutException(
                        string.Format(
                            CultureInfo.InvariantCulture,
                            "Process TimeOut ({0}): {1}",
                            settings.Timeout.Value,
                            fileName));
                }
            }
            else
            {
                process.WaitForExit();
            }

            redirectedStandardOutput = settings.RedirectStandardOutput
                ? process.GetStandardOutput()
                : null;

            // Return the exit code.
            return process.GetExitCode();
        }

        /// <summary>
        /// Starts the process resource that is specified by the filename and settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="redirectedStandardOutput">Returns process output if <see cref="ProcessSettings.RedirectStandardOutput"/> is true.
        /// Otherwise <c>null</c> is returned.</param>
        /// <param name="redirectedErrorOutput">Returns process error output if <see cref="ProcessSettings.RedirectStandardError"/> is true.
        /// Otherwise <c>null</c> is returned.</param>
        /// <returns>The exit code that the started process specified when it terminated.</returns>
        /// <example>
        /// <code>
        /// IEnumerable&lt;string&gt; redirectedStandardOutput;
        /// IEnumerable&lt;string&gt; redirectedErrorOutput;
        /// var exitCodeWithArgument =
        ///     StartProcess(
        ///         "ping",
        ///         new ProcessSettings {
        ///             Arguments = "localhost",
        ///             RedirectStandardOutput = true,
        ///             RedirectStandardError = true
        ///         },
        ///         out redirectedStandardOutput,
        ///         out redirectedErrorOutput
        ///     );
        ///
        /// // Output last line of process output.
        /// Information("Last line of output: {0}", redirectedStandardOutput.LastOrDefault());
        ///
        /// // Throw exception if anything was written to the standard error.
        /// if (redirectedErrorOutput.Any())
        /// {
        ///     throw new Exception(
        ///         string.Format(
        ///             "Errors occurred: {0}",
        ///             string.Join(", ", redirectedErrorOutput)));
        /// }
        ///
        /// // This should output 0 as valid arguments supplied
        /// Information("Exit code: {0}", exitCodeWithArgument);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static int StartProcess(
            this ICakeContext context,
            FilePath fileName,
            ProcessSettings settings,
            out IEnumerable<string> redirectedStandardOutput,
            out IEnumerable<string> redirectedErrorOutput)
        {
            var process = StartAndReturnProcess(context, fileName, settings);

            // Wait for the process to stop.
            if (settings.Timeout.HasValue)
            {
                if (!process.WaitForExit(settings.Timeout.Value))
                {
                    throw new TimeoutException(
                        string.Format(
                            CultureInfo.InvariantCulture,
                            "Process TimeOut ({0}): {1}",
                            settings.Timeout.Value,
                            fileName));
                }
            }
            else
            {
                process.WaitForExit();
            }

            redirectedStandardOutput = settings.RedirectStandardOutput
                ? process.GetStandardOutput()
                : null;

            redirectedErrorOutput = settings.RedirectStandardError
                ? process.GetStandardError()
                : null;

            // Return the exit code.
            return process.GetExitCode();
        }

        /// <summary>
        /// Starts the process resource that is specified by the filename and settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>The newly started process.</returns>
        /// <example>
        /// <code>
        /// using(var process = StartAndReturnProcess("ping", new ProcessSettings{ Arguments = "localhost" }))
        /// {
        ///     process.WaitForExit();
        ///     // This should output 0 as valid arguments supplied
        ///     Information("Exit code: {0}", process.GetExitCode());
        /// }
        /// </code>
        /// </example>
        /// <exception cref="ArgumentNullException"><paramref name="context"/>, <paramref name="fileName"/>, or <paramref name="settings"/>  is null.</exception>
        [CakeMethodAlias]
        public static IProcess StartAndReturnProcess(this ICakeContext context, FilePath fileName, ProcessSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (fileName == null)
            {
                throw new ArgumentNullException(nameof(fileName));
            }
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (!settings.NoWorkingDirectory)
            {
                // Set the working directory.
                var workingDirectory = settings.WorkingDirectory ?? context.Environment.WorkingDirectory;
                settings.WorkingDirectory = workingDirectory.MakeAbsolute(context.Environment);
            }

            // Start the process.
            var process = context.ProcessRunner.Start(fileName, settings);
            if (process == null)
            {
                throw new CakeException("Could not start process.");
            }

            return process;
        }

        /// <summary>
        /// Starts the process resource that is specified by the filename.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>The newly started process.</returns>
        /// <example>
        /// <code>
        /// using(var process = StartAndReturnProcess("ping"))
        /// {
        ///     process.WaitForExit();
        ///     // This should output 0 as valid arguments supplied
        ///     Information("Exit code: {0}", process.GetExitCode());
        /// }
        /// </code>
        /// </example>
        /// <exception cref="ArgumentNullException"><paramref name="context"/>, <paramref name="fileName"/> is null.</exception>
        [CakeMethodAlias]
        public static IProcess StartAndReturnProcess(this ICakeContext context, FilePath fileName)
        {
            return StartAndReturnProcess(context, fileName, new ProcessSettings());
        }
    }
}