using System;
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
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            // Get the working directory.
            var workingDirectory = settings.WorkingDirectory ?? context.Environment.WorkingDirectory;
            settings.WorkingDirectory = workingDirectory.MakeAbsolute(context.Environment);

            // Start the process.
            var process = context.ProcessRunner.Start(fileName, settings);
            if (process == null)
            {
                throw new CakeException("Could not start process.");
            }

            // Wait for the process to stop.
            process.WaitForExit();

            // Return the exit code.
            return process.GetExitCode();
        }
    }
}
