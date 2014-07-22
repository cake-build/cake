using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Core
{
    /// <summary>
    /// Represents a context for scripts.
    /// </summary>
    public interface ICakeContext
    {
        /// <summary>
        /// Gets the file system.
        /// </summary>
        /// <value>The file system.</value>
        IFileSystem FileSystem { get; }

        /// <summary>
        /// Gets the environment.
        /// </summary>
        /// <value>The environment.</value>
        ICakeEnvironment Environment { get; }

        /// <summary>
        /// Gets the globber.
        /// </summary>
        /// <value>The globber.</value>
        IGlobber Globber { get; }

        /// <summary>
        /// Gets the log.
        /// </summary>
        /// <value>The log.</value>
        ICakeLog Log { get; }

        /// <summary>
        /// Gets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        ICakeArguments Arguments { get; }

        /// <summary>
        /// Gets the process runner.
        /// </summary>
        /// <value>The process runner.</value>
        IProcessRunner ProcessRunner { get; }
    }
}