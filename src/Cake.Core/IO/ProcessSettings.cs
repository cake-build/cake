using System.Collections.Specialized;

namespace Cake.Core.IO
{
    /// <summary>
    /// Specifies a set of values that are used to start a process.
    /// </summary>
    public sealed class ProcessSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessSettings"/> class.
        /// </summary>
        public ProcessSettings()
        {
            EnvironmentVariables = new StringDictionary();
        }

        /// <summary>
        /// Gets or sets the set of command-line arguments to use when starting the application.
        /// </summary>
        /// <value>The set of command-line arguments to use when starting the application.</value>
        public ProcessArgumentBuilder Arguments { get; set; }

        /// <summary>
        /// Gets or sets the working directory for the process to be started.
        /// </summary>
        /// <value>The working directory for the process to be started.</value>
        public DirectoryPath WorkingDirectory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the output of an application is written to the <see cref="P:System.Diagnostics.Process.StandardOutput"/> stream.
        /// </summary>
        /// <value>true if output should be written to <see cref="P:System.Diagnostics.Process.StandardOutput"/>; otherwise, false. The default is false.</value>
        public bool RedirectStandardOutput { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the error output of an application is written to the
        /// <see cref="P:System.Diagnostics.Process.StandardError" /> stream.
        /// </summary>
        /// <value>
        /// true if error output should be written to <see cref="P:System.Diagnostics.Process.StandardError" />; otherwise,
        /// false. The default is false.
        /// </value>
        public bool RedirectStandardError { get; set; }

        /// <summary>
        /// Gets or sets optional timeout for process execution
        /// </summary>
        public int? Timeout { get; set; }

        /// <summary>
        /// Gets search paths for files, directories for temporary files, application-specific options, and other similar
        /// information.
        /// </summary>
        /// <value>
        /// A string dictionary that provides environment variables that apply to this process and child processes. The default is
        /// an empty StringDictionary.
        /// </value>
        public StringDictionary EnvironmentVariables { get; private set; }
    }
}