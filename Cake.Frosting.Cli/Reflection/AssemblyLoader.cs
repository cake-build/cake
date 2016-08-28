// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using System.Runtime.Loader;
using Cake.Core.IO;

namespace Cake.Frosting.Cli.Reflection
{
    internal sealed class AssemblyLoader : AssemblyLoadContext
    {
        private readonly IFileSystem _fileSystem;
        private readonly DirectoryPath _root;

        public AssemblyLoader(IFileSystem fileSystem, DirectoryPath root)
        {
            _fileSystem = fileSystem;
            _root = root;
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            // Does the file exist on disk?
            var file = new FilePath(assemblyName.Name).ChangeExtension(".dll");
            var path = _root.CombineWithFilePath(file);
            if (_fileSystem.Exist(path))
            {
                // Try loading it in this context.
                return LoadFromAssemblyPath(path.FullPath);
            }

            // Load the assembly in the default assembly load context.
            return Assembly.Load(assemblyName);
        }
    }
}
