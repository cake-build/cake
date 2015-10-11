namespace Cake.Core.IO
{
    /// <summary>
    /// Represents a process runner.
    /// </summary>
    public interface IProcessRunner
    {
        /// <summary>
        /// Starts a process using the specified information.
        /// </summary>
        /// <param name="filePath">The file name such as an application or document with which to start the process.</param>
        /// <param name="settings">The information about the process to start.</param>
        /// <returns>A process handle.</returns>
        IProcess Start(FilePath filePath, ProcessSettings settings);
    }
}