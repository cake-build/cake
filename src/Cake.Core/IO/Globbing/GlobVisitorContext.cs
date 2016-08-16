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
        private readonly Func<IDirectory, bool> _predicate;

        internal DirectoryPath Path { get; private set; }

        public IFileSystem FileSystem { get; }

        public ICakeEnvironment Environment { get; }

        public List<IFileSystemInfo> Results { get; }

        public GlobVisitorContext(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            Func<IDirectory, bool> predicate)
        {
            FileSystem = fileSystem;
            Environment = environment;
            _predicate = predicate;
            Results = new List<IFileSystemInfo>();
            _pathParts = new LinkedList<string>();
        }

        public void AddResult(IFileSystemInfo path)
        {
            Results.Add(path);
        }

        public void Push(string path)
        {
            _pathParts.AddLast(path);
            Path = GenerateFullPath();
        }

        public string Pop()
        {
            var last = _pathParts.Last;
            _pathParts.RemoveLast();
            Path = GenerateFullPath();
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