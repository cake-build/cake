// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Cake.Core.IO
{
    /// <summary>
    /// A collection of <see cref="DirectoryPath"/>.
    /// </summary>
    public sealed class DirectoryPathCollection : IEnumerable<DirectoryPath>
    {
        private readonly PathComparer _comparer;
        private readonly HashSet<DirectoryPath> _paths;

        /// <summary>
        /// Gets the number of directories in the collection.
        /// </summary>
        /// <value>The number of directories in the collection.</value>
        public int Count
        {
            get { return _paths.Count; }
        }

        internal PathComparer Comparer
        {
            get { return _comparer; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryPathCollection"/> class.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        public DirectoryPathCollection(PathComparer comparer)
            : this(Enumerable.Empty<DirectoryPath>(), comparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryPathCollection"/> class.
        /// </summary>
        /// <param name="paths">The paths.</param>
        /// <param name="comparer">The comparer.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="comparer"/> is <c>null</c>.</exception>
        public DirectoryPathCollection(IEnumerable<DirectoryPath> paths, PathComparer comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            _comparer = comparer;
            _paths = new HashSet<DirectoryPath>(paths, comparer);
        }

        /// <summary>
        /// Adds the specified path to the collection.
        /// </summary>
        /// <param name="path">The path to add.</param>
        /// <returns>
        ///   <c>true</c> if the path was added; <c>false</c> if the path was already present.
        /// </returns>
        public bool Add(DirectoryPath path)
        {
            return _paths.Add(path);
        }

        /// <summary>
        /// Adds the specified paths to the collection.
        /// </summary>
        /// <param name="paths">The paths to add.</param>
        public void Add(IEnumerable<DirectoryPath> paths)
        {
            if (paths == null)
            {
                throw new ArgumentNullException("paths");
            }
            foreach (var path in paths)
            {
                _paths.Add(path);
            }
        }

        /// <summary>
        /// Removes the specified path from the collection.
        /// </summary>
        /// <param name="path">The path to remove.</param>
        /// <returns>
        ///   <c>true</c> if the path was removed; <c>false</c> if the path was not found in the collection.
        /// </returns>
        public bool Remove(DirectoryPath path)
        {
            return _paths.Remove(path);
        }

        /// <summary>
        /// Removes the specified paths from the collection.
        /// </summary>
        /// <param name="paths">The paths to remove.</param>
        public void Remove(IEnumerable<DirectoryPath> paths)
        {
            if (paths == null)
            {
                throw new ArgumentNullException("paths");
            }
            foreach (var path in paths)
            {
                _paths.Remove(path);
            }
        }

        /// <summary>Adds a path to the collection.</summary>
        /// <param name="collection">The collection.</param>
        /// <param name="path">The path to add.</param>
        /// <returns>A new <see cref="DirectoryPathCollection"/> that contains the provided path as
        /// well as the paths in the original collection.</returns>
        public static DirectoryPathCollection operator +(DirectoryPathCollection collection, DirectoryPath path)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            return new DirectoryPathCollection(collection, collection.Comparer) { path };
        }

        /// <summary>Adds multiple paths to the collection.</summary>
        /// <param name="collection">The collection.</param>
        /// <param name="paths">The paths to add.</param>
        /// <returns>A new <see cref="DirectoryPathCollection"/> with the content of both collections.</returns>
        public static DirectoryPathCollection operator +(DirectoryPathCollection collection, IEnumerable<DirectoryPath> paths)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            return new DirectoryPathCollection(collection, collection.Comparer) { paths };
        }

        /// <summary>
        /// Removes a path from the collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="path">The path to remove.</param>
        /// <returns>A new <see cref="DirectoryPathCollection"/> that do not contain the provided path.</returns>
        public static DirectoryPathCollection operator -(DirectoryPathCollection collection, DirectoryPath path)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            var result = new DirectoryPathCollection(collection, collection.Comparer);
            result.Remove(path);
            return result;
        }

        /// <summary>
        /// Removes multiple paths from the collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="paths">The paths to remove.</param>
        /// <returns>A new <see cref="DirectoryPathCollection"/> that do not contain the provided paths.</returns>
        public static DirectoryPathCollection operator -(DirectoryPathCollection collection, IEnumerable<DirectoryPath> paths)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            var result = new DirectoryPathCollection(collection, collection.Comparer);
            result.Remove(paths);
            return result;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<DirectoryPath> GetEnumerator()
        {
            return _paths.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
