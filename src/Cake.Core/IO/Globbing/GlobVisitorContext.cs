// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Cake.Core.IO.Globbing
{
    internal sealed class GlobVisitorContext
    {
        private readonly List<string> _pathParts;
        private readonly Func<IDirectory, bool> _directoryPredicate;
        private readonly Func<IFile, bool> _filePredicate;

        internal DirectoryPath Path { get; private set; }
        public IFileSystem FileSystem { get; }
        public ICakeEnvironment Environment { get; }
        public List<IFileSystemInfo> Results { get; }

        public GlobVisitorContext(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            Func<IDirectory, bool> directoryPredicate,
            Func<IFile, bool> filePredicate)
        {
            FileSystem = fileSystem;
            Environment = environment;
            _directoryPredicate = directoryPredicate;
            _filePredicate = filePredicate;
            Results = new List<IFileSystemInfo>();
            _pathParts = new List<string>();
        }

        public void AddResult(IFileSystemInfo path)
        {
            Results.Add(path);
        }

        public void Push(string path)
        {
            _pathParts.Add(path);
            Path = GenerateFullPath();
        }

        public string Pop()
        {
            var last = _pathParts[_pathParts.Count - 1];
            _pathParts.RemoveAt(_pathParts.Count - 1);
            Path = GenerateFullPath();
            return last;
        }

        private DirectoryPath GenerateFullPath()
        {
            if (_pathParts.Count > 0 && _pathParts[0] == @"\\")
            {
                // UNC path
                var path = string.Concat(@"\\", string.Join(@"\", _pathParts.Skip(1)));
                return new DirectoryPath(path);
            }
            if (_pathParts.Count > 0 && _pathParts[0] == "/")
            {
                // Unix root path
                var path = string.Concat("/", string.Join("/", _pathParts.Skip(1)));
                return new DirectoryPath(path);
            }
            else
            {
                // Regular path
                var path = string.Join("/", _pathParts);
                if (string.IsNullOrWhiteSpace(path))
                {
                    path = "./";
                }
                return new DirectoryPath(path);
            }
        }

        public bool ShouldTraverse(IDirectory info)
        {
            if (_directoryPredicate != null)
            {
                return _directoryPredicate(info);
            }
            return true;
        }

        public bool ShouldInclude(IFile file)
        {
            if (_filePredicate != null)
            {
                return _filePredicate(file);
            }
            return true;
        }
    }
}