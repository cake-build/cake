// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;

namespace Cake.Core.IO.Globbing
{
    internal sealed class GlobVisitorContext
    {
        private readonly LinkedList<string> _pathParts;
        private readonly List<IFileSystemInfo> _results;
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly Func<IDirectory, bool> _predicate;
        private DirectoryPath _path;

        internal DirectoryPath Path
        {
            get { return _path; }
        }

        public IFileSystem FileSystem
        {
            get { return _fileSystem; }
        }

        public ICakeEnvironment Environment
        {
            get { return _environment; }
        }

        public List<IFileSystemInfo> Results
        {
            get { return _results; }
        }

        public GlobVisitorContext(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            Func<IDirectory, bool> predicate)
        {
            _fileSystem = fileSystem;
            _environment = environment;
            _predicate = predicate;
            _results = new List<IFileSystemInfo>();
            _pathParts = new LinkedList<string>();
        }

        public void AddResult(IFileSystemInfo path)
        {
            _results.Add(path);
        }

        public void Push(string path)
        {
            _pathParts.AddLast(path);
            _path = GenerateFullPath();
        }

        public string Pop()
        {
            var last = _pathParts.Last;
            _pathParts.RemoveLast();
            _path = GenerateFullPath();
            return last.Value;
        }

        private DirectoryPath GenerateFullPath()
        {
            var path = string.Join("/", _pathParts);
            if (string.IsNullOrWhiteSpace(path))
            {
                path = "./";
            }
            return new DirectoryPath(path);
        }

        public bool ShouldTraverse(IDirectory info)
        {
            if (_predicate != null)
            {
                return _predicate(info);
            }
            return true;
        }
    }
}
