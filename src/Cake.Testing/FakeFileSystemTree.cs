// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Testing
{
    internal sealed class FakeFileSystemTree
    {
        private readonly PathComparer _comparer;
        private readonly FakeDirectory _root;

        public PathComparer Comparer
        {
            get { return _comparer; }
        }

        public FakeFileSystemTree(ICakeEnvironment environment)
        {
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }
            if (environment.WorkingDirectory == null)
            {
                throw new ArgumentException("Working directory not set.");
            }
            if (environment.WorkingDirectory.IsRelative)
            {
                throw new ArgumentException("Working directory cannot be relative.");
            }
            _comparer = new PathComparer(environment.IsUnix());

            _root = new FakeDirectory(this, "/");
            _root.Create();
        }

        public FakeDirectory CreateDirectory(DirectoryPath path)
        {
            return CreateDirectory(new FakeDirectory(this, path));
        }

        public FakeDirectory CreateDirectory(FakeDirectory directory)
        {
            var path = directory.Path;
            var queue = new Queue<string>(path.Segments);

            FakeDirectory current = null;
            var children = _root.Content;

            while (queue.Count > 0)
            {
                // Get the segment.
                var currentSegment = queue.Dequeue();
                var parent = current;

                // Calculate the current path.
                path = parent != null ? parent.Path.Combine(currentSegment) : new DirectoryPath(currentSegment);

                if (!children.Directories.ContainsKey(path))
                {
                    current = queue.Count == 0 ? directory : new FakeDirectory(this, path);
                    current.Parent = parent ?? _root;
                    current.Hidden = false;
                    children.Add(current);
                }
                else
                {
                    current = children.Directories[path];
                }

                current.Exists = true;
                children = current.Content;
            }

            return directory;
        }

        public void CreateFile(FakeFile file)
        {
            // Get the directory that the file exists in.
            var directory = FindDirectory(file.Path.GetDirectory());
            if (directory == null)
            {
                file.Exists = false;
                throw new DirectoryNotFoundException(string.Format("Could not find a part of the path '{0}'.", file.Path.FullPath));
            }

            if (!directory.Content.Files.ContainsKey(file.Path))
            {
                // Add the file to the directory.
                file.Exists = true;
                directory.Content.Add(file);
            }
        }

        public void DeleteDirectory(FakeDirectory fakeDirectory, bool recursive)
        {
            var root = new Stack<FakeDirectory>();
            var result = new Stack<FakeDirectory>();

            if (fakeDirectory.Exists)
            {
                root.Push(fakeDirectory);
            }

            while (root.Count > 0)
            {
                var node = root.Pop();
                result.Push(node);

                var directories = node.Content.Directories;

                if (directories.Count > 0 && !recursive)
                {
                    throw new IOException("The directory is not empty.");
                }

                foreach (var child in directories)
                {
                    root.Push(child.Value);
                }
            }

            while (result.Count > 0)
            {
                var directory = result.Pop();

                var files = directory.Content.Files.Select(x => x).ToArray();
                if (files.Length > 0 && !recursive)
                {
                    throw new IOException("The directory is not empty.");
                }

                foreach (var file in files)
                {
                    // Delete the file.
                    DeleteFile(file.Value);
                }

                // Delete the directory.
                directory.Parent.Content.Remove(directory);
                directory.Exists = false;
            }
        }

        public void DeleteFile(FakeFile file)
        {
            if (!file.Exists)
            {
                throw new FileNotFoundException("File does not exist.", file.Path.FullPath);
            }

            // Find the directory.
            var directory = FindDirectory(file.Path.GetDirectory());

            // Remove the file from the directory.
            directory.Content.Remove(file);

            // Reset all properties.
            file.Exists = false;
            file.Content = null;
            file.ContentLength = 0;
        }

        public FakeDirectory FindDirectory(DirectoryPath path)
        {
            // Want the root?
            if (path.Segments.Length == 0)
            {
                return _root;
            }

            var queue = new Queue<string>(path.Segments);

            FakeDirectory current = null;
            var children = _root.Content;

            while (queue.Count > 0)
            {
                // Set the parent.
                var parent = current;

                // Calculate the current path.
                var segment = queue.Dequeue();
                path = parent != null ? parent.Path.Combine(segment) : new DirectoryPath(segment);

                // Find the current path.
                if (!children.Directories.ContainsKey(path))
                {
                    return null;
                }

                current = children.Directories[path];
                children = current.Content;
            }

            return current;
        }

        public FakeFile FindFile(FilePath path)
        {
            var directory = FindDirectory(path.GetDirectory());
            if (directory != null)
            {
                if (directory.Content.Files.ContainsKey(path))
                {
                    return directory.Content.Files[path];
                }
            }
            return null;
        }

        public void CopyFile(FakeFile file, FilePath destination, bool overwrite)
        {
            if (!file.Exists)
            {
                throw new FileNotFoundException("File do not exist.");
            }

            // Already exists?
            var destinationFile = FindFile(destination);
            if (destinationFile != null)
            {
                if (!overwrite)
                {
                    const string format = "{0} exists and overwrite is false.";
                    var message = string.Format(format, destination.FullPath);
                    throw new IOException(message);
                }
            }

            // Directory exists?
            var directory = FindDirectory(destination.GetDirectory());
            if (directory == null || !directory.Exists)
            {
                throw new DirectoryNotFoundException("The destination path {0} do not exist.");
            }

            // Make sure the file exist.
            if (destinationFile == null)
            {
                destinationFile = new FakeFile(this, destination);
            }

            // Copy the data from the original file to the destination.
            using (var input = file.OpenRead())
            using (var output = destinationFile.OpenWrite())
            {
                input.CopyTo(output);
            }
        }

        public void MoveFile(FakeFile fakeFile, FilePath destination)
        {
            // Copy the file to the new location.
            CopyFile(fakeFile, destination, false);

            // Delete the original file.
            fakeFile.Delete();
        }
    }
}
