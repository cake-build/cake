using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Cake.Core.IO.Globbing.Nodes;

namespace Cake.Core.IO.Globbing
{
    internal sealed class GlobVisitor
    {
        public void VisitRecursiveWildcardSegment(RecursiveWildcardSegment node, GlobVisitorContext context)
        {
            var path = context.FileSystem.GetDirectory(context.FullPath);
            if (context.FileSystem.Exist(path.Path))
            {
                foreach (var candidate in FindCandidates(path.Path, node, context, SearchScope.Recursive, includeFiles: false))
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
                if (node.Next == null)
                {
                    var segmentPath = node.Render();

                    // Directories
                    var directoryPath = context.FileSystem.GetDirectory(new DirectoryPath(context.FullPath).Combine(segmentPath));
                    if (directoryPath.Exists)
                    {
                        context.AddResult(directoryPath);
                    }

                    // Files
                    var filePath = context.FileSystem.GetFile(new DirectoryPath(context.FullPath).CombineWithFilePath(segmentPath));
                    if (filePath.Exists)
                    {
                        context.AddResult(filePath);
                    }
                }
                else
                {
                    context.Push(node.Render());
                    node.Next.Accept(this, context);
                    context.Pop();
                }
            }
            else
            {
                if (node.Tokens.Count > 1)
                {
                    var path = context.FileSystem.GetDirectory(context.FullPath);
                    if (path.Exists)
                    {
                        var next = node.Next;
                        foreach (var candidate in FindCandidates(path.Path, node, context, SearchScope.Current))
                        {
                            if (next != null)
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
            var path = context.FileSystem.GetDirectory(context.FullPath);
            if (context.FileSystem.Exist(path.Path))
            {
                if (node.Next == null)
                {
                    context.AddResult(path);
                }

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
            if (string.IsNullOrWhiteSpace(node.Drive))
            {
                // Get the drive from the working directory.
                var workingDirectory = context.Environment.WorkingDirectory;
                var root = workingDirectory.FullPath.Split('/').First();
                context.Push(root);
            }
            else
            {
                context.Push(node.Drive + ":");
            }
            node.Next.Accept(this, context);
            context.Pop();
        }

        private static IReadOnlyList<IFileSystemInfo> FindCandidates(
            DirectoryPath path, 
            GlobNode node, 
            GlobVisitorContext context, 
            SearchScope option,
            bool includeFiles = true,
            bool includeDirectories = true)
        {
            var result = new List<IFileSystemInfo>();
            var current = context.FileSystem.GetDirectory(path);
            var expression = new Regex("^" + node.Render() + "$", context.Options);

            // Directories
            if (includeDirectories)
            {
                foreach (var directory in current.GetDirectories("*", option))
                {
                    var lastPath = directory.Path.Segments.Last();
                    if (expression.IsMatch(lastPath))
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
                    if (expression.IsMatch(lastPath))
                    {
                        result.Add(file);
                    }
                }                
            }

            return result;
        }
    }
}
