using System.Collections.Generic;

namespace Cake.Core.IO
{
    /// <summary>
    /// Represents a directory.
    /// </summary>
    public interface IDirectory
    {
        /// <summary>
        /// Gets the path to the directory.
        /// </summary>
        /// <value>The path.</value>
        DirectoryPath Path { get; }

        /// <summary>
        /// Gets the name of the directory.
        /// </summary>
        /// <value>The directory's name.</value>
        string Name { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IDirectory"/> exists.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the directory exists; otherwise, <c>false</c>.
        /// </value>
        bool Exists { get; }

        /// <summary>
        /// Creates the directory.
        /// </summary>
        void Create();

        /// <summary>
        /// Deletes the directory.
        /// </summary>
        /// <param name="recursive">Will perform a recursive delete if set to <c>true</c>.</param>
        void Delete(bool recursive);

        /// <summary>
        /// Gets directories matching the specified filter and scope.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="scope">The scope.</param>
        /// <returns>Directories matching the filter and scope.</returns>
        IEnumerable<IDirectory> GetDirectories(string filter, SearchScope scope);

        /// <summary>
        /// Gets files matching the specified filter and scope.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="scope">The scope.</param>
        /// <returns>Files matching the specified filter and scope.</returns>
        IEnumerable<IFile> GetFiles(string filter, SearchScope scope);
    }
}