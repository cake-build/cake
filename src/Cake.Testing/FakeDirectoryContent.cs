// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.Testing
{
    internal sealed class FakeDirectoryContent
    {
        private readonly FakeDirectory _owner;
        private readonly Dictionary<DirectoryPath, FakeDirectory> _directories;
        private readonly Dictionary<FilePath, FakeFile> _files;

        public FakeDirectory Owner
        {
            get { return _owner; }
        }

        public IReadOnlyDictionary<DirectoryPath, FakeDirectory> Directories
        {
            get { return _directories; }
        }

        public IReadOnlyDictionary<FilePath, FakeFile> Files
        {
            get { return _files; }
        }

        public FakeDirectoryContent(FakeDirectory owner, PathComparer comparer)
        {
            _owner = owner;
            _directories = new Dictionary<DirectoryPath, FakeDirectory>(comparer);
            _files = new Dictionary<FilePath, FakeFile>(comparer);
        }

        public void Add(FakeDirectory directory)
        {
            _directories.Add(directory.Path, directory);
        }

        public void Add(FakeFile file)
        {
            _files.Add(file.Path, file);
        }

        public void Remove(FakeDirectory directory)
        {
            _directories.Remove(directory.Path);
        }

        public void Remove(FakeFile file)
        {
            _files.Remove(file.Path);
        }
    }
}
