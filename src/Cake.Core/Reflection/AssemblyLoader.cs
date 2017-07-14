// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using Cake.Core.IO;
using Cake.Core.Polyfill;

namespace Cake.Core.Reflection
{
    internal sealed class AssemblyLoader : IAssemblyLoader
    {
        private readonly ICakeEnvironment _environment;
        private readonly IFileSystem _fileSystem;
        private readonly AssemblyVerifier _verifier;

        public AssemblyLoader(ICakeEnvironment environment, IFileSystem fileSystem, AssemblyVerifier verifier)
        {
            _environment = environment;
            _fileSystem = fileSystem;
            _verifier = verifier;
        }

        public Assembly Load(AssemblyName assemblyName)
        {
            if (assemblyName == null)
            {
                throw new ArgumentNullException(nameof(assemblyName));
            }

            return Assembly.Load(assemblyName);
        }

        public Assembly Load(FilePath path, bool verify)
        {
            var assembly = AssemblyHelper.LoadAssembly(_environment, _fileSystem, path);
            if (verify)
            {
                _verifier.Verify(assembly);
            }
            return assembly;
        }
    }
}