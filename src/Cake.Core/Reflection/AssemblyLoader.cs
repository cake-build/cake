// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Polyfill;

namespace Cake.Core.Reflection
{
    /// <summary>
    /// Responsible for loading assemblies.
    /// </summary>
    public sealed class AssemblyLoader : IAssemblyLoader
    {
        private readonly ICakeEnvironment _environment;
        private readonly IFileSystem _fileSystem;
        private readonly IAssemblyVerifier _verifier;
        private readonly ICakeLog _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyLoader"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="verifier">The assembly verifier.</param>
        /// <param name="log">The cake log.</param>
        public AssemblyLoader(ICakeEnvironment environment, IFileSystem fileSystem, IAssemblyVerifier verifier, ICakeLog log)
        {
            _environment = environment;
            _fileSystem = fileSystem;
            _verifier = verifier;
            _log = log;
        }

        /// <inheritdoc/>
        public Assembly Load(AssemblyName assemblyName)
        {
            return AssemblyHelper.LoadAssembly(assemblyName);
        }

        /// <inheritdoc/>
        public Assembly Load(FilePath path, bool verify)
        {
            var assembly = AssemblyHelper.LoadAssembly(_environment, _fileSystem, _log, path);
            if (verify && assembly != null)
            {
                _verifier.Verify(assembly);
            }
            return assembly;
        }
    }
}