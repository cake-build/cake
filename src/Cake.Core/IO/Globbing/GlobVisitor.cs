﻿using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core.IO.Globbing.Nodes;

namespace Cake.Core.IO.Globbing
{
    internal sealed class GlobVisitor
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;

        public GlobVisitor(IFileSystem fileSystem, ICakeEnvironment environment)
        {
            _fileSystem = fileSystem;
            _environment = environment;
        }

        public IEnumerable<IFileSystemInfo> Walk(GlobNode node, Func<IDirectory, bool> predicate)
        {
            var context = new GlobVisitorContext(_fileSystem, _environment, predicate);
            node.Accept(this, context);
            return context.Results;
        }

        public void VisitRecursiveWildcardSegment(RecursiveWildcardSegment node, GlobVisitorContext context)
        {
            var path = context.FileSystem.GetDirectory(context.Path);
            if (context.FileSystem.Exist(path.Path))
            {
                // Check if folders match.
                var candidates = new List<IFileSystemInfo>();
                candidates.Add(path);
                candidates.AddRange(FindCandidates(path.Path, node, context, SearchScope.Recursive, includeFiles: false));

                foreach (var candidate in candidates)
                {
                    var pushed = false;
                    if (context.Path.FullPath != candidate.Path.FullPath)
                    {
                        context.Push(candidate.Path.FullPath.Substring(path.Path.FullPath.Length + 1));
                        pushed = true;
                    }

                    if (node.Next != null)
                    {
                        node.Next.Accept(this, context);
                    }
                    else
                    {
                        context.AddResult(candidate);
                    }

                    if (pushed)
                    {
                        context.Pop();
                    }
                }
            }
        }

        public void VisitRelativeRoot(RelativeRoot node, GlobVisitorContext context)
        {
            context.Push(context.Environment.WorkingDirectory.FullPath);
            node.Next.Accept(this, context);
            context.Pop();
        }

        public void VisitSegment(PathSegment node, GlobVisitorContext context)
        {
            if (node.IsIdentifier)
            {
                // Get the (relative) path to the current node.
                var segment = node.GetPath();

                // Get a directory that matches this segment.
                // This might be a file but we can't be sure so we need to check.
                var directoryPath = context.Path.Combine(segment);
                var directory = context.FileSystem.GetDirectory(directoryPath);

                // Should we not traverse this directory?
                if (directory.Exists && !context.ShouldTraverse(directory))
                {
                    return;
                }

                if (node.Next == null)
                {
                    if (directory.Exists)
                    {
                        // Directory
                        context.AddResult(directory);
                    }
                    else
                    {
                        // Then it must be a file (if it exist).
                        var filePath = context.Path.CombineWithFilePath(segment);
                        var file = context.FileSystem.GetFile(filePath);
                        if (file.Exists)
                        {
                            // File
                            context.AddResult(file);
                        }
                    }
                }
                else
                {
                    context.Push(node.GetPath());
                    node.Next.Accept(this, context);
                    context.Pop();
                }
            }
            else
            {
                if (node.Tokens.Count > 1)
                {
                    var path = context.FileSystem.GetDirectory(context.Path);
                    if (path.Exists)
                    {
                        foreach (var candidate in FindCandidates(path.Path, node, context, SearchScope.Current))
                        {
                            if (node.Next != null)
                            {
                                context.Push(candidate.Path.FullPath.Substring(path.Path.FullPath.Length + 1));
                                node.Next.Accept(this, context);
                                context.Pop();
                            }
                            else
                            {
                                context.AddResult(candidate);
                            }
                        }
                    }
                }
            }
        }

        public void VisitUnixRoot(UnixRoot node, GlobVisitorContext context)
        {
            context.Push(string.Empty);
            node.Next.Accept(this, context);
            context.Pop();
        }

        public void VisitWildcardSegmentNode(WildcardSegment node, GlobVisitorContext context)
        {
            var path = context.FileSystem.GetDirectory(context.Path);
            if (context.FileSystem.Exist(path.Path))
            {
                foreach (var candidate in FindCandidates(path.Path, node, context, SearchScope.Current))
                {
                    context.Push(candidate.Path.FullPath.Substring(path.Path.FullPath.Length + 1));
                    if (node.Next != null)
                    {
                        node.Next.Accept(this, context);
                    }
                    else
                    {
                        context.AddResult(candidate);
                    }
                    context.Pop();
                }
            }
        }

        public void VisitWindowsRoot(WindowsRoot node, GlobVisitorContext context)
        {
            context.Push(node.Drive + ":");
            node.Next.Accept(this, context);
            context.Pop();
        }

        private static IEnumerable<IFileSystemInfo> FindCandidates(
            DirectoryPath path,
            MatchableNode node,
            GlobVisitorContext context,
            SearchScope option,
            bool includeFiles = true,
            bool includeDirectories = true)
        {
            var result = new List<IFileSystemInfo>();
            var current = context.FileSystem.GetDirectory(path);

            // Directories
            if (includeDirectories)
            {
                foreach (var directory in current.GetDirectories("*", option))
                {
                    var lastPath = directory.Path.Segments.Last();
                    if (node.IsMatch(lastPath) && context.ShouldTraverse(directory))
                    {
                        result.Add(directory);
                    }
                }
            }

            // Files
            if (includeFiles)
            {
                foreach (var file in current.GetFiles("*", option))
                {
                    var lastPath = file.Path.Segments.Last();
                    if (node.IsMatch(lastPath))
                    {
                        result.Add(file);
                    }
                }
            }

            return result;
        }
    }
}
