namespace Cake.Core.IO
{
    /// <summary>
    /// Specifies a set of values that are used to start a process.
    /// </summary>
    public sealed class ProcessSettings
    {
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
        /// Gets or sets optional timeout for process execution
        /// </summary>
        public int? Timeout { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether process output will be suppressed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if process output will be suppressed; otherwise, <c>false</c>.
        /// </value>
        public bool Silent { get; set; }
    }
}