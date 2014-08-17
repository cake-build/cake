using Cake.Core.IO;

namespace Cake.Common
{
    /// <summary>
    /// Specifies a set of values that are used when you start a process.
    /// </summary>
    public sealed class ProcessSettings
    {
        /// <summary>
        /// Gets or sets the set of command-line arguments to use when starting the application.
        /// </summary>
        /// <value>The set of command-line arguments to use when starting the application.</value>
        public string Arguments { get; set; }

        /// <summary>
        /// Gets or sets the working directory for the process to be started.
        /// </summary>
        /// <value>The working directory for the process to be started.</value>
        public DirectoryPath WorkingDirectory { get; set; }
    }
}
