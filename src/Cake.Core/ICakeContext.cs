using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Core
{
    /// <summary>
    /// Represents a context for scripts and script aliases.
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

        /// <summary>
        /// Gets resolver by tool name
        /// </summary>
        /// <param name="toolName">resolver tool name</param>
        /// <returns>IToolResolver for tool</returns>
        IToolResolver GetToolResolver(string toolName);

        /// <summary>
        /// Gets the registry.
        /// </summary>
        /// <value>The registry.</value>
        IRegistry Registry { get; }
    }
}