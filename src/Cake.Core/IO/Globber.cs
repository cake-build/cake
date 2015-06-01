///////////////////////////////////////////////////////////////////////
// Portions of this code was ported from glob-js by Kevin Thompson.
// https://github.com/kthompson/glob-js
///////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Cake.Core.IO.Globbing;
using Cake.Core.IO.Globbing.Nodes;

namespace Cake.Core.IO
{
    /// <summary>
    /// Responsible for file system globbing.
    /// </summary>
    public sealed class Globber : IGlobber
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly RegexOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="Globber"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        public Globber(IFileSystem fileSystem, ICakeEnvironment environment)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException("fileSystem");
            }
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }
            _fileSystem = fileSystem;
            _environment = environment;
            _options = RegexOptions.Singleline;

            if (!_environment.IsUnix())
            {
                // On non unix systems, we should ignore case.
                _options |= RegexOptions.IgnoreCase;
            }
        }

        /// <summary>
        /// Returns <see cref="Path" /> instances matching the specified pattern.
        /// </summary>
        /// <param name="pattern">The pattern to match.</param>
        /// <param name="predicate">The predicate used to filter directories based on file system information.</param>
        /// <returns>
        ///   <see cref="Path" /> instances matching the specified pattern.
        /// </returns>
        public IEnumerable<Path> Match(string pattern, Func<IFileSystemInfo, bool> predicate)
        {
            var scanner = new Scanner(pattern);
            var parser = new Parser(scanner, _environment);
            var path = parser.Parse();

            var rootNodes = new List<Node>();
            while (path.Count > 0)
            {
                // Pop the first path item.
                var segment = path[0];
                path.RemoveAt(0);

                if (segment.IsWildcard)
                {
                    path.Insert(0, segment);
                    break;
                }
                rootNodes.Add(segment);
            }

            // Fix up the tree.
            var newRoot = FixRootNode(rootNodes);
            if (newRoot != null)
            {
                rootNodes[0] = newRoot;
            }

            // Ge the root.
            var rootPath = string.Join("/", rootNodes.Select(x => x.Render()));

            // Nothing left in the path?
            if (path.Count == 0)
            {
                return GetPath(rootPath);
            }

            var rootDirectory = new DirectoryPath(rootPath);

            // Walk the root and return the unique results.
            var segments = new Stack<Node>(((IEnumerable<Node>)path).Reverse());
            var results = Walk(rootDirectory, segments, predicate ?? (p => true));
            return new HashSet<Path>(results, new PathComparer(_environment.IsUnix())).ToArray();
        }

        private Node FixRootNode(List<Node> rootNodes)
        {
            // Windows root?
            var windowsRoot = rootNodes[0] as WindowsRoot;
            if (windowsRoot != null)
            {
                // No drive?
                if (string.IsNullOrWhiteSpace(windowsRoot.Drive))
                {
                    // Get the drive from the working directory.
                    var workingDirectory = _environment.WorkingDirectory;
                    var root = workingDirectory.FullPath.Split('/').First();
                    return new IdentifierNode(root);
                }
            }

            // Relative root?
            var relativeRoot = rootNodes[0] as RelativeRoot;
            if (relativeRoot != null)
            {
                // Get the drive from the working directory.
                var workingDirectory = _environment.WorkingDirectory;
                return new IdentifierNode(workingDirectory.FullPath);
            }

            return null;
        }

        private IEnumerable<Path> GetPath(string rootPath)
        {
            // Is this an existing file?
            var rootFilePath = new FilePath(rootPath);
            if (_fileSystem.Exist(rootFilePath))
            {
                return new Path[] { rootFilePath };
            }

            // Is this an existing directory?
            var rootDirectoryPath = new DirectoryPath(rootPath);
            if (_fileSystem.Exist(rootDirectoryPath))
            {
                return new Path[] { rootDirectoryPath };
            }

            // Neither an existing file or directory.
            return new Path[] { };
        }

        private List<Path> Walk(DirectoryPath rootPath, Stack<Node> segments, Func<IFileSystemInfo, bool> predicate)
        {
            var results = new List<Path>();
            var segment = segments.Pop();

            var expression = new Regex("^" + segment.Render() + "$", _options);
            var isDirectoryWildcard = false;

            if (segment is WildcardSegmentNode)
            {
                segments.Push(segment);
                isDirectoryWildcard = true;
            }

            // Get all files and folders.
            var root = _fileSystem.GetDirectory(rootPath);
            if (!root.Exists)
            {
                return results;
            }
            var rootFullPath = PathCollapser.Collapse(rootPath);
            foreach (var directory in root.GetDirectories("*", SearchScope.Current, predicate))
            {
                var part = directory.Path.FullPath.Substring(rootFullPath.Length + 1);
                var pathTest = expression.IsMatch(part);

                var subWalkCount = 0;

                if (isDirectoryWildcard)
                {
                    // Walk recursivly down the segment.
                    var nextSegments = new Stack<Node>(segments.Reverse());
                    var subwalkResult = Walk(directory.Path, nextSegments, predicate);
                    if (subwalkResult.Count > 0)
                    {
                        results.AddRange(subwalkResult);
                    }

                    subWalkCount++;
                }

                // Check without directory wildcard.
                if (segments.Count > subWalkCount && (subWalkCount == 1 || pathTest))
                {
                    // Walk the next segment in the list.
                    var nextSegments = new Stack<Node>(segments.Skip(subWalkCount).Reverse());
                    var subwalkResult = Walk(directory.Path, nextSegments, predicate);
                    if (subwalkResult.Count > 0)
                    {
                        results.AddRange(subwalkResult);
                    }
                }

                // Got a match?
                if (pathTest && segments.Count == 0)
                {
                    results.Add(directory.Path);
                }
            }

            foreach (var file in root.GetFiles("*", SearchScope.Current, predicate))
            {
                var part = file.Path.FullPath.Substring(rootFullPath.Length + 1);
                var pathTest = expression.IsMatch(part);

                // Got a match?
                if (pathTest && segments.Count == 0)
                {
                    results.Add(file.Path);
                }
                else if (pathTest)
                {
                    /////////////////////////////////////////////////////////////B
                    // We got a match, but we still have segments left.
                    // Is the next part a directory wild card?
                    /////////////////////////////////////////////////////////////

                    var nextNode = segments.Peek();
                    if (nextNode is WildcardSegmentNode)
                    {
                        var nextSegments = new Stack<Node>(segments.Skip(1).Reverse());
                        var subwalkResult = Walk(root.Path, nextSegments, predicate);
                        if (subwalkResult.Count > 0)
                        {
                            results.AddRange(subwalkResult);
                        }
                    }
                }
            }
            return results;
        }
    }
}
