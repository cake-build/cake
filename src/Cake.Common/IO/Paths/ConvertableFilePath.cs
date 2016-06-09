// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Core.IO;

namespace Cake.Common.IO.Paths
{
    /// <summary>
    /// Represents a <see cref="FilePath" /> that can be easily converted.
    /// </summary>
    public sealed class ConvertableFilePath
    {
        private readonly FilePath _path;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertableFilePath"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        internal ConvertableFilePath(FilePath path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            _path = path;
        }

        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <value>The actual path.</value>
        public FilePath Path
        {
            get { return _path; }
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ConvertableFilePath"/> to <see cref="FilePath"/>.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The result of the conversion. </returns>
        public static implicit operator FilePath(ConvertableFilePath path)
        {
            if (path == null)
            {
                return null;
            }
            return path.Path;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ConvertableFilePath"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator string(ConvertableFilePath path)
        {
            if (path == null)
            {
                return null;
            }
            return path.Path.FullPath;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return _path.FullPath;
        }
    }
}
