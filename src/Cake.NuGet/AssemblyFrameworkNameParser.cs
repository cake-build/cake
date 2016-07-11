// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Runtime.Versioning;
using Cake.Core.IO;

namespace Cake.NuGet
{
    /// <summary>
    /// Represents an object that parses the segments of a filepath for a .Net framework name
    /// </summary>
    public class AssemblyFrameworkNameParser : IAssemblyFrameworkNameParser
    {
        private static readonly FrameworkName _unsupportedFrameworkName = new FrameworkName("Unsupported", new Version(0, 0));
        private readonly IFrameworkNameParser _frameworkNameParser;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyFrameworkNameParser"/> class.
        /// </summary>
        /// <param name="frameworkNameParser">The framework name parser.</param>
        public AssemblyFrameworkNameParser(IFrameworkNameParser frameworkNameParser)
        {
            _frameworkNameParser = frameworkNameParser;
        }

        /// <summary>
        /// Parses the framework name from assembly file path.
        /// </summary>
        /// <param name="path">The assembly file path.</param>
        /// <returns>the parsed framework name, or <c>null</c> when path contains no folders.</returns>
        public FrameworkName Parse(FilePath path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            // The path for a reference might look like this for assembly foo.dll:
            // foo.dll
            // sub\foo.dll
            // {FrameworkName}{Version}\foo.dll
            // {FrameworkName}{Version}\sub1\foo.dll
            // {FrameworkName}{Version}\sub1\sub2\foo.dll
            return ParseFromDirectoryPath(path.GetDirectory());
        }

        private FrameworkName ParseFromDirectoryPath(DirectoryPath path)
        {
            var queue = new Queue<string>(path.Segments);
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                var parsedFxName = _frameworkNameParser.ParseFrameworkName(current);
                if (parsedFxName != _unsupportedFrameworkName || queue.Count == 0)
                {
                    return parsedFxName;
                }
            }
            return null;
        }
    }
}
