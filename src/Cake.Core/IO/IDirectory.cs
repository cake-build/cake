using System;
using System.Collections.Generic;

namespace Cake.Core.IO
{
    /// <summary>
    /// Represents a directory.
    /// </summary>
    public interface IDirectory : IFileSystemInfo
    {
        /// <summary>
        /// Gets the path to the directory.
        /// </summary>
        /// <value>The path.</value>
        new DirectoryPath Path { get; }

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
        /// Gets directories matching the specified filter and scope, with option to exclude hidden directories
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="wherePredicate">Filters returned directories based on predicate</param>
        /// <returns>Directories matching the filter and scope.</returns>
        IEnumerable<IDirectory> GetDirectories(string filter, SearchScope scope, Func<IFileSystemInfo, bool> wherePredicate);

        /// <summary>
        /// Gets directories matching the specified filter and scope, with option to exclude hidden directories
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="wherePredicate">Filters returned directories based on predicate</param>
        /// <param name="predicateFiltered">Callback if directory gets filtered by wherePredicate</param>
        /// <returns>Directories matching the filter and scope.</returns>
        IEnumerable<IDirectory> GetDirectories(string filter, SearchScope scope, Func<IFileSystemInfo, bool> wherePredicate, Action<IFileSystemInfo> predicateFiltered);

        /// <summary>
        /// Gets files matching the specified filter and scope.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="scope">The scope.</param>
        /// <returns>Files matching the specified filter and scope.</returns>
        IEnumerable<IFile> GetFiles(string filter, SearchScope scope);

        /// <summary>
        /// Gets files matching the specified filter and scope.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="wherePredicate">Filters returned files based on predicate</param>
        /// <returns>Files matching the specified filter and scope.</returns>
        IEnumerable<IFile> GetFiles(string filter, SearchScope scope, Func<IFileSystemInfo, bool> wherePredicate);

        /// <summary>
        /// Gets files matching the specified filter and scope.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="wherePredicate">Filters returned files based on predicate</param>
        /// <param name="predicateFiltered">Callback if file gets filtered by wherePredicate</param>
        /// <returns>Files matching the specified filter and scope.</returns>
        IEnumerable<IFile> GetFiles(string filter, SearchScope scope, Func<IFileSystemInfo, bool> wherePredicate, Action<IFileSystemInfo> predicateFiltered);
    }
}