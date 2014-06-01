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
using Cake.Core.IO.Globbing.Nodes.Roots;

namespace Cake.Core.IO
{
    public sealed class Globber : IGlobber
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;

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
        }

        public IEnumerable<Path> Match(string pattern)
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
            var rootDirectory = new DirectoryPath(string.Join("/", rootNodes.Select(x => x.Render())));

            // Nothing left in the path?
            if (path.Count == 0)
            {
                // We have an absolute path with no wild cards.
                return new Path[] { rootDirectory };
            }

            // Walk the root and return the unique results.
            var segments = new Stack<Node>(((IEnumerable<Node>)path).Reverse());
            var results = Walk(rootDirectory, segments);
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
                    var root = workingDirectory.FullPath.Split(new[] { '/' }).First();
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

        private List<Path> Walk(DirectoryPath rootPath, Stack<Node> segments)
        {
            var results = new List<Path>();
            var segment = segments.Pop();

            var expression = new Regex(segment.Render() + "$", RegexOptions.Singleline);
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

            foreach (var directory in root.GetDirectories("*", SearchScope.Current))
            {
                var part = directory.Path.FullPath.Substring(root.Path.FullPath.Length + 1);
                var pathTest = expression.IsMatch(part);

                var subWalkCount = 0;

                if (isDirectoryWildcard)
                {
                    // Walk recursivly down the segment.
                    var nextSegments = new Stack<Node>(segments.Reverse());
                    var subwalkResult = Walk(directory.Path, nextSegments);
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
                    var subwalkResult = Walk(directory.Path, nextSegments);
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

            foreach (var file in root.GetFiles("*", SearchScope.Current))
            {
                var part = file.Path.FullPath.Substring(root.Path.FullPath.Length + 1);
                var pathTest = expression.IsMatch(part);

                // Got a match?
                if (pathTest && segments.Count == 0)
                {
                    results.Add(file.Path);
                }
                else if(pathTest)
                {
                    /////////////////////////////////////////////////////////////B
                    // We got a match, but we still have segments left.
                    // Is the next part a directory wild card?
                    /////////////////////////////////////////////////////////////

                    var nextNode = segments.Peek();
                    if (nextNode is WildcardSegmentNode)
                    {
                        var nextSegments = new Stack<Node>(segments.Skip(1).Reverse());
                        var subwalkResult = Walk(root.Path, nextSegments);
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
